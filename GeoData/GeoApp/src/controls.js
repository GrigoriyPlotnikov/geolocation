// @ts-check

// idea by JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/

import { Navigator } from './navigator.js'
import { createElement } from './component.js'

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
  }

  /**
   * Called when the element is inserted into the DOM. Used to fetch the template and make the content
   */
  async connectedCallback() {
    this.innerText = '';
    /** @jsx createElement */
    const ControlsTemplate = (props) => (
      <ul>
        {Object.keys(props.screens).map(key => (
          <li><a href={'#' + key}> {props.screens[key].head}</a></li>
        ))}
      </ul>
    );

    this.appendChild(ControlsTemplate({ screens: this._deck.jsx_screens }));

    this.querySelectorAll('a').forEach(anchor => {
      const route = anchor.hash.substr(1);
      this._controlRef[route] = anchor;
      anchor.addEventListener('click', (ev) => { ev.preventDefault(); this._deck.jumpTo(route) });
    });
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