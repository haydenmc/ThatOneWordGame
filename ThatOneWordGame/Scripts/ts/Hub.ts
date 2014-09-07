interface SignalR {
	gameHub: any;
}

class Hub {
	private application: Application;
	private hub = $.connection.gameHub;
	private connectionId: string;
	public ready: boolean = false;

	constructor(application: Application) {
		this.application = application;

		this.hub.client.notify = (message: string) => {
			console.log("SERVER: " + message);
		};
		this.hub.client.updateLeaderboard = (players: Array<Player>) => this.updateLeaderboard(players);
		this.hub.client.newRound = (round: Round) => this.newRound(round);
		this.hub.client.roundUpdate = (round: Round) => this.roundUpdate(round);
	}

	public connect(callback: () => void) {
		if (window.debug) {
			$.connection.hub.logging = true;
		}
		$.connection.hub.start().done(() => {
			this.ready = true;
			if (callback) {
				callback();
			}
			this.hub.server.getId().done((id: string) => {
				console.log("Your connection ID: " + id);
				this.connectionId = id;
			});
		});
	}

	public getConnectionId(): string {
		return this.connectionId;
	}

	/* Client-side methods */
	public updateLeaderboard(players: Array<Player>) {
		this.application.getDataModel().updatePlayers(players);
	}

	public newRound(round: Round) {
		this.application.getPlayUI().newRound(round);
	}

	public roundUpdate(round: Round) {
		this.application.getPlayUI().roundUpdate(round);
	}

	/* Server-side methods */
	public setName(name: string, callback: () => void) {
		this.hub.server.setName(name).done(callback);
	}

	public tryLetter(letter: string, callback: () => boolean) {
		this.hub.server.tryLetter(letter).done(callback);
	}
}