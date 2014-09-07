class PlayUI extends Component {
	constructor(application: Application) {
		super(application, "PlayUI");
	}

	public show() {
		super.show();
		var playpane = <HTMLElement>this.getElement().getElementsByClassName("playpane")[0];
		var input = (<HTMLElement>playpane.getElementsByClassName("wordentry")[0]).getElementsByTagName("input")[0];
		input.addEventListener("keyup", () => {
			this.submitLetter(input.value);
			input.disabled = true;
			input.value = "";
		});
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

	public newRound(r: Round) {
		var playpane = <HTMLElement>this.getElement().getElementsByClassName("playpane")[0];
		var input = (<HTMLElement>playpane.getElementsByClassName("wordentry")[0]).getElementsByTagName("input")[0];
		(<HTMLElement>playpane.getElementsByClassName("wordentry")[0]).getElementsByTagName("span")[0].innerHTML = r.Word;
		if (r.Player.ConnectionId == this.getApplication().getHub().getConnectionId()) {
			input.disabled = false;
			playpane.getElementsByTagName("h1")[0].innerHTML = "It's your turn!";
			if (r.Word.length == 0) {
				playpane.getElementsByTagName("h2")[0].innerHTML = "Enter the first letter of a word.";
			} else {
				playpane.getElementsByTagName("h2")[0].innerHTML = "Enter the next letter of a word.";
			}
		} else {
			input.disabled = true;
			playpane.getElementsByTagName("h1")[0].innerHTML = r.Player.Name + "'s turn!";
		}
	}

	public roundUpdate(r: Round) {
		this.newRound(r);
	}

	public submitLetter(l: string) {
		this.getApplication().getHub().tryLetter(l, null);
	}
}