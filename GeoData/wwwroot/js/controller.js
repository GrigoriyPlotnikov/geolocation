// @ts-check

// initial code from JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/

import { Navigator } from './navigator.js'

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
     * @type {Object.<string, HTMLAnchorElement}
     * */
    this._controlRef = {}

  }

  /**
   * Called when the element is inserted into the DOM. Used to fetch the template and wire into the related Navigator instance.
   */
  async connectedCallback() {
  }

  formLinks() {
    this.innerHTML = '';
    const host = document.createElement('div');
    const ul = document.createElement('ul');
    host.appendChild(ul);

    if (this._deck.screens) {
      for (const root in this._deck.screens) {
        const li = document.createElement('li');
        const a = document.createElement('a');
        a.href = `#${root}`
        a.innerText = this._deck.screens[root].head;
        //a.addEventListener('click', (e) => {
        //  e.preventDefault();
        //  this._deck.jumpTo(a.href.substr(1));
        //});
        this._controlRef[root] = a;
        li.appendChild(a);
        ul.appendChild(li);
      }
    }

    this.appendChild(host);
    this.refreshState();
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
      this._controlRef[this._deck.route].classList.remove('disabled');
    }

    if (this._deck.routePrevious && this._controlRef[this._deck.routePrevious]) {
      this._controlRef[this._deck.routePrevious].classList.add('disabled');
    }
  }
}

/** Register the custom slide-controls element */
export const registerControls = () => customElements.define('screen-controls', Controls);