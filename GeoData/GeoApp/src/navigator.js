// @ts-check

// initial code from JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/

import { loadScreens } from './loadScreens.js'
import { createElement } from './component.js'
import { Home } from './screens/home.js'
import { Router } from './router.js'
import { Animator } from './animator.js'
import { Screen } from './screen.js'

/**
 * @typedef {object} JsxScreen
 * @property {string} head head
 * @property {string} title title
 * @property {HTMLElement} element
 * */

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
    this._route = null;
    /**
     * The previous known route
     * @type {string}
     */
    this._routePrevious = null;
    /** 
     *  Known screens
     *  @type {Object.<string, Screen>}
     * */
    this._screens = {};

    /** 
     *  Known jsx screens
     *  @type {Object.<string, JsxScreen>}
     * */
    this._jsx_screens = {
      "home": { element: Home(), head: 'О приложении', title: 'Экран информации о приложении' }
    };
    /**
     * Custom event raised when the current screen changes
     * @type {CustomEvent}
     */
    //todo: wtf CustomEvent maybe pass routes screens
    this.screenChangedEvent = new CustomEvent('screenchanged', {
      bubbles: true,
      cancelable: false
    });
    this.screensLoadedEvent = new CustomEvent('screensloaded', {
      bubbles: true,
      cancelable: false
    });
    this._router.eventSource.addEventListener('routechanged', () => {
      if (this._route !== this._router.getRoute()) {
        const route = this._router.getRoute();
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
    if (this._route === route)
      return;

    if (this._screens[route]) {
      this._routePrevious = this._route;
      this._route = route;
      this.innerHTML = '';
      this.appendChild(this.currentScreen.html);
      if (this._routePrevious)
        this._router.setRoute(route);
      document.title = `${this.currentScreen.title}`;
      this.dispatchEvent(this.screenChangedEvent);
      this.currentScreen.dataBindExecute();
      if (this._animator.animationReady) {
        this._animator.endAnimation(this.querySelector('div'));
      }
    }

    if (this._jsx_screens[route]) {
      this._routePrevious = this._route;
      this._route = route;
      this.innerHTML = '';
      this.appendChild(this._jsx_screens[route].element);
      if (this._routePrevious)
        this._router.setRoute(route);
      document.title = this.querySelectorAll("title")[0].innerText;
      this.dispatchEvent(this.screenChangedEvent);
    }
  }

  /**
   * Get the list of observed attributes
   * @returns {string[]} The list of attributes to watch
   */
  static get observedAttributes() {
    return ['start'];
  }

  /**
   * Called when an attribute changes
   * @param {string} attrName 
   * @param {string} oldVal 
   * @param {string} newVal 
   */
  async attributeChangedCallback(attrName, oldVal, newVal) {
    if (attrName === 'start') {
      if (oldVal !== newVal) {
        for (const scr of await loadScreens()) {
          this._screens[scr.route] = scr;
        }
        this.dispatchEvent(this.screensLoadedEvent);
        let route = this._router.getRoute();
        if (route) {
          this.jumpTo(route);
        } else {
          this.jumpTo(newVal);
        }
        this._title = document.querySelectorAll('title')[0];
      }
    }
  }


  /**
  * Current screen
  * @returns {Screen} The current screen
  */
  get currentScreen() {
    return this._screens ? this._screens[this._route] : null;
  }

  /**
  * All known screens
  * @returns {Object.<string,Screen>} Screens dictionary
  */
  get screens() {
    return this._screens;
  }

  /**
  * All known screens
  * @returns {Object.<string, JsxScreen>} Screens dictionary
  */
  get jsx_screens() {
    return this._jsx_screens;
  }

  /**
   * Get the current route
   * @returns {string} The current route name
   */
  get route() {
    return this._route;
  }

  /**
   * Get the previous route
   * @returns {string} The previous route name
   */
  get routePrevious() {
    return this._routePrevious;
  }

}

/**
 * Register the custom screen-deck component
 */
export const registerDeck = () => customElements.define('screen-deck', Navigator);