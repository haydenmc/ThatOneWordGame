using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using ThatOneWordGame.Models;
using System.Timers;

namespace ThatOneWordGame.Hubs
{
	public class GameHub : Hub
	{
		private static bool Playing = false;
		private static Round CurrentRound = null;
		private readonly static List<Round> RoundHistory = new List<Round>();
		private readonly static Dictionary<string, Player> Players = new Dictionary<string, Player>();

		public override Task OnConnected()
		{
			Players.Add(Context.ConnectionId, new Player() {
				ConnectionId = Context.ConnectionId,
				Name = "Player " + Players.Count,
				Playing = false,
				Score = 0
			});
			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			Players.Remove(Context.ConnectionId);
			_updateLeaderboard();
			return base.OnDisconnected(stopCalled);
		}

		// ----------------------------
		// Server-side methods
		// ----------------------------

		public string GetId()
		{
			return Context.ConnectionId;
		}

		/// <summary>
		/// Sets client's name
		/// </summary>
		/// <param name="name">Name of the client</param>
		public void SetName(string name)
		{
			var player = Players[Context.ConnectionId];
			player.Name = name;
			if (!player.Playing)
			{
				player.Playing = true;
			}
			_updateLeaderboard();
			if (!Playing)
			{
				Playing = true;
				_nextRound();
			}
		}

		/// <summary>
		/// Check to see if a word is valid!
		/// </summary>
		/// <param name="word"></param>
		public bool TryLetter(string letter)
		{
			letter = letter.Substring(0, 1).ToUpper();
			CurrentRound.Word += letter; 
			using (var context = new WordsEntities())
			{
				// If we put an invalid character, end the round now and punish the player.
				var count = context.Words.Where(w => w.Word1.ToUpper().StartsWith(CurrentRound.Word.ToUpper())).Count();
				if (count <= 0)
				{
					CurrentRound.Score = -1 * _calculatePoints(CurrentRound.Word);
					CurrentRound.Player.Score += CurrentRound.Score;
					Clients.All.EndRound(CurrentRound);
					_scheduleNextRound();
					return false;
				}

				// If we completed a word, end the round and award the player.
				var endWordCount = context.Words.Where(w => w.Word1.ToUpper().Equals(CurrentRound.Word.ToUpper())).Count();
				if (endWordCount > 0)
				{
					CurrentRound.Score = _calculatePoints(CurrentRound.Word);
					CurrentRound.Player.Score += CurrentRound.Score;
					Clients.All.EndRound(CurrentRound);
					_scheduleNextRound();
					return true;
				}

				// Otherwise, move to the next player.
				CurrentRound.Player = _nextPlayer();
				Clients.All.RoundUpdate(CurrentRound);
				return true;
			}
		}

		/// <summary>
		/// Player wants to end the round with their submission. Award points if valid word, subtract if not.
		/// </summary>
		/// <param name="letter"></param>
		/// <returns></returns>
		//public bool EndRound(string letter)
		//{
		//	if (Context.ConnectionId != CurrentRound.Player.ConnectionId) return false;
		//	letter = letter.Substring(0, 1).ToUpper();
		//	CurrentRound.Word += letter;
		//	using (var context = new WordsEntities())
		//	{
		//		var count = context.Words.Where(w => w.Word1.ToUpper().Equals(CurrentRound.Word.ToUpper())).Count();
		//		if (count > 0)
		//		{
		//			CurrentRound.Score = _calculatePoints(CurrentRound.Word);
		//			RoundHistory.Add(CurrentRound);
		//			Clients.All.EndRound(CurrentRound);
		//			return true;
		//		}
		//		else
		//		{
		//			CurrentRound.Score = -1 * _calculatePoints(CurrentRound.Word);
		//			RoundHistory.Add(CurrentRound);
		//			Clients.All.EndRound(CurrentRound);
		//			return false;
		//		}
		//	}
		//}

		private Player _nextPlayer()
		{
			int lastPlayerIndex = 0;
			if (CurrentRound != null)
			{
				lastPlayerIndex = Players.Values.ToList().IndexOf(CurrentRound.Player);
				lastPlayerIndex += 1;
				if (lastPlayerIndex >= Players.Values.Count)
				{
					lastPlayerIndex = 0;
				}
			}
			return Players.Values.ToList()[lastPlayerIndex];
		}

		private void _scheduleNextRound()
		{
			var timer = new Timer();
			timer.Interval = 3000;
			timer.Elapsed += timer_NextRound_Elapsed;
			timer.AutoReset = false;
			timer.Start();
		}

		void timer_NextRound_Elapsed(object sender, ElapsedEventArgs e)
		{
			_nextRound();
		}

		/// <summary>
		/// Player wants to continue the round with their submission. If they have an invalid word, end it.
		/// </summary>
		/// <param name="letter"></param>
		/// <returns></returns>
		//public bool ContinueRound(string letter)
		//{
		//	letter = letter.Substring(0, 1).ToUpper();
		//	CurrentRound.Word += letter;
		//	using (var context = new WordsEntities())
		//	{
		//		var count = context.Words.Where(w => w.Word1.ToUpper().Equals(CurrentRound.Word.ToUpper())).Count();
		//		if (count > 0)
		//		{

		//		}
		//	}
		//}

		private int _calculatePoints(string word)
		{
			return word.Length * 100;
		}

		// ----------------------------
		// Client-side methods
		// ----------------------------

		/// <summary>
		/// Updates the leader board on all clients
		/// </summary>
		private void _updateLeaderboard()
		{
			Clients.All.UpdateLeaderboard(Players.Values.Where(x => x.Playing == true).OrderByDescending(x => x.Score).ToList());
		}

		private void _nextRound()
		{
			if (CurrentRound != null)
			{
				RoundHistory.Add(CurrentRound);
			}
			var startingPlayer = _nextPlayer();
			CurrentRound = new Round();
			CurrentRound.Player = startingPlayer;
			
			Clients.All.NewRound(CurrentRound);
		}
	}

	public class Player
	{
		public string ConnectionId { get; set; }
		public string Name { get; set; }
		public bool Playing { get; set; }
		public long Score { get; set; }
	}

	public class Round
	{
		public Round()
		{
			Word = "";
		}
		public string Word { get; set; }
		public Player Player { get; set; }
		public int Score { get; set; }
	}
}