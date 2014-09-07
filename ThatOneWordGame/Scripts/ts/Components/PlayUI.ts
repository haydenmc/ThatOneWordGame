class PlayUI extends Component {
	constructor(application: Application) {
		super(application, "PlayUI");
	}

	public updateLeaderboard(players: Array<Player>) {
		var playerList = <HTMLElement>(this.getElement().getElementsByClassName("playerlist")[0]);
		playerList.innerHTML = "";
		for (var i = 0; i < players.length; i++) {
			var listitem = document.createElement("li");
			var name = document.createElement("span");
			name.classList.add("name");
			name.innerHTML = players[i].Name;
			var score = document.createElement("span");
			score.classList.add("score");
			score.innerHTML = players[i].Score.toString();
			listitem.appendChild(name);
			listitem.appendChild(score);
			playerList.appendChild(listitem);
		}
	}
}