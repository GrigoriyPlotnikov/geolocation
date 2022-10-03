///initial code from JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/
///revorked to nestle screens 

import { loadScreens } from "./loadScreens.js"
import { Router } from "./router.js"
import { Animator } from "./animator.js"
import { Screen } from "./screen.js"

/**
 * The main class that handles rendering the screen deck
 * @extends {HTMLElement}
 */
export class Navigator extends HTMLElement {
  /**
 * Create an instance of the custom navigator element
 */
  constructor() {
    super();
    /**
     * The related animation control
     * @type {Animator}
     */
    this._animator = new Animator();
    /**
     * The related router control
     * @type {Router}
     */
    this._router = new Router();
    /**
     * The last known route
     * @type {string}
     */
    this._route = this._router.getRoute();
    /**
     * Custom event raised when the current screen changes
     * @type {CustomEvent}
     */
    this.screenChangedEvent = new CustomEvent("screenchanged", {
      bubbles: true,
      cancelable: false
    });
    this._router.eventSource.addEventListener("routechanged", () => {
      if (this._route !== this._router.getRoute()) {
        let route = this._router.getRoute();
        if (route) {
          this.jumpTo(route);
        }
      }
    });
  }

  /**
   * Main scrren navigation: jump to specific screen
   * @param {string} route The path of the screen to navigate to
   */
  jumpTo(route) {
    if (this._animator.transitioning) {
      return;
    }
    if (this._screens[route]) {
      this._route = route;
      this.innerHTML = '';
      this.appendChild(this.currentScreen.html);
      this._router.setRoute(route);
      document.title = `${this.currentScreen.title}`;
      this.dispatchEvent(this.screenChangedEvent);
      if (this._animator.animationReady) {
        this._animator.endAnimation(this.querySelector("div"));
      }
    }
  }

  /**
   * Get the list of observed attributes
   * @returns {string[]} The list of attributes to watch
   */
  static get observedAttributes() {
    return ["start"];
  }

  /**
   * Called when an attribute changes
   * @param {string} attrName 
   * @param {string} oldVal 
   * @param {string} newVal 
   */
  async attributeChangedCallback(attrName, oldVal, newVal) {
    if (attrName === "start") {
      if (oldVal !== newVal) {
        this._screens = await loadScreens();
        let route = this._router.getRoute();
        if (route) {
          this.jumpTo(route);
        } else {
          this.jumpTo(newVal);
        }
        this._title = document.querySelectorAll("title")[0];
      }
    }
  }


  /**
  * Current slide
  * @returns {Screen} The current screen
  */
  get currentScreen() {
    return this._screens ? this._screens[this._route] : null;
  }
}

/**
 * Register the custom screen-deck component
 */
export const registerDeck = () => customElements.define('screen-deck', Navigator);