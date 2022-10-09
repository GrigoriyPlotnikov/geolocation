// @ts-check

// idea by JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/

import { Navigator } from './navigator.js'
import { DataBinding } from "./dataBinding.js"

/**
 * Custom element that renders controls to navigate the deck
 * @extends {HTMLElement}
 */
export class Controls extends HTMLElement {

  /**
   * Create a new instance of controls
   */
  constructor() {
    super();

    /**
     * The related Navigator instance (deck) to control
     * @type {Navigator}
     */
    this._deck = null;

    /**
     * direct links to control nodes
     * @type {Object.<string, HTMLAnchorElement>}
     * */
    this._controlRef = {}

    /**
     * Data binding helper
     * @type {DataBinding}
     */
    this._dataBinding = new DataBinding();
  }

  /**
   * Called when the element is inserted into the DOM. Used to fetch the template
   */
  async connectedCallback() {
    const response = await fetch(`./templates/controls.html`);
    this._template = await response.text();
  }

  /**
   * Called when navigator have screens
   *  used to from ui and wire into the related Navigator instance.
   * */
  formLinks() {
    this.innerHTML = '';
    const host = document.createElement('div');
    host.innerHTML = this._template;

    /**
     * @typedef {object} Screenlink
     * @property {string} route route
     * @property {string} head head
     */

    /**
     * @typedef {object} ScreenlinkContext
     * @property {Screenlink[]} screens 
     */

    /**
     * Context for data bind
     * @type {ScreenlinkContext}
     */
    const context = {
      screens: /** @type {Screenlink[]} **/ []
    };

    if (this._deck.screens) {
      for (const root in this._deck.screens) {
        context.screens.push({ route: root, head: this._deck.screens[root].head});
      }
    }

    this._dataBinding.bindAll(host, context);

    host.querySelectorAll('a').forEach(anchor => {
      const route = anchor.hash.substr(1);
      this._controlRef[route] = anchor;
      anchor.addEventListener('click', (ev) => { ev.preventDefault(); this._deck.jumpTo(route) });
    });

    this.appendChild(host);
  }

  /**
   * Get the list of attributes to watch
   * @returns {string[]} List of observable attributes
   */
  static get observedAttributes() {
    return ['deck'];
  }

  /**
   * Called when the attribute is set
   * @param {string} attrName Name of the attribute that was set
   * @param {string} oldVal The old attribute value
   * @param {string} newVal The new attribute value
   */
  async attributeChangedCallback(attrName, oldVal, newVal) {
    if (attrName === 'deck') {
      if (oldVal !== newVal) {
        this._deck = /** @type {Navigator} */(document.getElementById(newVal));
        this._deck.addEventListener('screenchanged', () => this.refreshState());
        this._deck.addEventListener('screensloaded', () => this.formLinks());
      }
    }
  }

  /**
   * Enables/disables buttons based on screen in the deck
   */
  refreshState() {
    if (this._deck.route && this._controlRef[this._deck.route]) {
      this._controlRef[this._deck.route].classList.add('disabled');
    }

    if (this._deck.routePrevious && this._controlRef[this._deck.routePrevious]) {
      this._controlRef[this._deck.routePrevious].classList.remove('disabled');
    }
  }
}

/** Register the custom slide-controls element */
export const registerControls = () => customElements.define('screen-controls', Controls);