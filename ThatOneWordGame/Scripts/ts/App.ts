class Application {
	public static instance: Application;
	constructor() {
		Application.instance = this;
		var connecting = new Connecting(this);
		connecting.show();
	}
}

window.onload = function () {
	new Application();
};