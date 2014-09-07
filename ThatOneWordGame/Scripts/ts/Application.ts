class Application {
	public static instance: Application;
	private dataModel: DataModel;
	private hub: Hub;
	private playui: PlayUI;
	constructor() {
		Application.instance = this;
		this.hub = new Hub(this);
		this.dataModel = new DataModel(this);
		this.playui = new PlayUI(this);
		var connecting = new Connecting(this);
		connecting.show();
		this.hub.connect(() => {
			connecting.hide();
			var hello = new Hello(this);
			hello.show();
		});
	}

	public getHub(): Hub {
		return this.hub;
	}

	public getDataModel(): DataModel {
		return this.dataModel;
	}

	public getPlayUI(): PlayUI {
		return this.playui;
	}
}

window.onload = function () {
	new Application();
};