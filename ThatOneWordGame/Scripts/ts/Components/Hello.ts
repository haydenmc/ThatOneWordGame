class Hello extends Component {
	constructor(application: Application) {
		super(application, "Hello");
		this.getElement().getElementsByTagName("form")[0].addEventListener("submit", (e: Event) => {
			e.preventDefault();
			this.submit();
		});
	}

	private submit(): void {
		this.getElement().getElementsByTagName("form")[0].disabled = true;
		(<HTMLElement>this.getElement().getElementsByClassName("spinner_container_32")[0]).style["display"] = "block";
		this.getApplication().getHub().setName(this.getElement().getElementsByTagName("input")[0].value, () => {
			this.hide();
			this.getApplication().getPlayUI().show();
		});
	}
}