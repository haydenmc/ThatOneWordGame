class DataModel {
	private application: Application;
	private players: Array<Player>;

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
}