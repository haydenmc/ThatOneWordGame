/**
 * The Component class is a super-class of all components.
 * It provides default implementations for things like showing, hiding, and animating.
 * More specific behaviors are to be implemented by each individual component.
 */
class Component {
	private application: Application;
	private component_id: string;
	private element: HTMLElement;
	private animator: Animator;

	constructor(application: Application, component_id: string) {
		// Get the component ID, grab the HTML
		this.component_id = component_id;
		this.element = <HTMLDivElement> document.getElementById("component_" + this.component_id).cloneNode(true);
		this.element.classList.remove("COMPONENT");
		this.element.id = '';

		// Make sure we're associated with an application instance...
		if (application != null) {
			this.application = application;
		} else {
			this.application = Application.instance; // Default instance if undefined
		}
	}

	public getApplication(): Application {
		return this.application;
	}

	public getElement(): HTMLElement {
		return this.element;
	}

	public show(): void {
		this.element = <HTMLElement>document.body.appendChild(this.element);
		Animator.animate(this, "in") // IN is the default event for showing.
	}

	public hide(): void {
		Animator.animate(this, "out") // OUT is the default event for showing.
		// We have 300ms to animate out before we're taken off the DOM Tree.
		setTimeout(() => {
			this.element = <HTMLElement>document.body.removeChild(this.element);
		}, 300);
	}
} 