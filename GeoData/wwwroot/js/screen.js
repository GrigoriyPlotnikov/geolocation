// @ts-check

//initial code from JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/

import { DataBinding } from "./dataBinding.js"

/** 
 * Represents a screen
 * */
export class Screen {

  /**
   * @constructor
   * @param {string} text - The content of the screen
   * @param {string} route - The route of the screen
   */
  constructor(text, route) {
    /**
     * Route for the screen
     * @type {string}
     */
    this._route = route;
    /** 
     * Internal text representation of the screen
     * @type {string}
     */
    this._text = text;
    /**
     * Context for embedded scripts
     * @type {object}
     */
    this._context = {};
    /**
     * Data binding helper
     * @type {DataBinding}
     */
    this._dataBinding = new DataBinding();
    /** 
     * The HTML DOM hosting the screen contents
     * @type {HTMLDivElement}
     */
    this._html = document.createElement('div');
    this._html.innerHTML = text;
    /**
     * The title of the screen
     * @type {string}
     */
    this._title = this._html.querySelectorAll("title")[0].innerText;

    /**
     * The link name of the screen
     * @type {string}
     */
    this._head = this._html.querySelector("meta[name='controlhead']").getAttribute("content");

    /** @type{NodeListOf<HTMLElement>} */
    const transition = (this._html.querySelectorAll("transition"));
    if (transition.length) {
      /**
       * The name of the animation to use for transitions
       * @type {string}
       */
      this._transition = transition[0].innerText;
    }
    else {
      this._transition = null;
    }
    // execute any scripts
    const script = this._html.querySelector("script");
    if (script) {
      this._dataBinding.executeInContext(script.innerText, this._context, true);
      this._dataBinding.bindAll(this._html, this._context);
    }
  }

  /** 
   * The screen transition
   * @returns {string} The transition name
   */
  get transition() {
    return this._transition;
  }

  /** 
   * The screen title
   * @returns {string} The screen title
   */
  get title() {
    return this._title;
  }

  /** 
   * The link name of the screen
   * @returns {string} The name for screen links
   */
  get head() {
    return this._head;
  }

  /** 
   * The screen route
   * @returns {string} The screen route
   */
  get route() {
    return this._route;
  }

  /**
   * The HTML DOM node for the slide
   * @returns {HTMLDivElement} The HTML content
   */
  get html() {
    return this._html;
  }
}