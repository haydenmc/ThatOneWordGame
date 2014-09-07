interface SignalR {
	gameHub: any;
}

class Hub {
	private application: Application;
	private hub = $.connection.gameHub;
	public ready: boolean = false;

	constructor(application: Application) {
		this.application = application;

		this.hub.client.notify = (message: string) => {
			console.log("SERVER: " + message);
		};
		this.hub.client.updateLeaderboard = (players: Array<Player>) => this.updateLeaderboard(players);
	}

	/* Client-side methods */
	public connect(callback: () => void) {
		if (window.debug) {
			$.connection.hub.logging = true;
		}
		$.connection.hub.start().done(() => {
			this.ready = true;
			if (callback) {
				callback();
			}
		});
	}

	public updateLeaderboard(players: Array<Player>) {
		this.application.getDataModel().updatePlayers(players);
	}

	/* Server-side methods */
	public setName(name: string, callback: () => void) {
		this.hub.server.setName(name).done(callback);
	}

	public tryWord(word: string, callback: () => boolean) {
		this.hub.server.tryWord(word).done(callback);
	}
}