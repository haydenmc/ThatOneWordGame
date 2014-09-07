using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using ThatOneWordGame.Models;

namespace ThatOneWordGame.Hubs
{
	public class GameHub : Hub
	{
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
		}

		/// <summary>
		/// Check to see if a word is valid!
		/// </summary>
		/// <param name="word"></param>
		public bool TryWord(string word)
		{
			using (var context = new WordsEntities())
			{
				var count = context.Words.Where(w => w.Word1.ToUpper().Equals(word.ToUpper())).Count();
				return (count > 0);
			}
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
	}

	public class Player
	{
		public string ConnectionId { get; set; }
		public string Name { get; set; }
		public bool Playing { get; set; }
		public long Score { get; set; }
	}
}