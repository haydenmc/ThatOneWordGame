class DataModel {
	private application: Application;
	private players: Array<Player>;
	private rounds: Array<Round>;

	constructor(application: Application) {
		this.application = application;
		this.players = new Array<Player>();
	}

	public getPlayers(): Array<Player> {
		return this.players;
	}

	public updatePlayers(players: Array<Player>): void {
		this.players = players;
		this.application.getPlayUI().updateLeaderboard(players);
	}

	public updateRounds(rounds: Array<Round>): void {
		this.rounds = rounds;
		this.application.getPlayUI().updateRoundHistory(rounds);
	}
}