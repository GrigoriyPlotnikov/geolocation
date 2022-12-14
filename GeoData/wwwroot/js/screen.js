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

    /**
     * @typedef {object} SceenContext
     * @property {HTMLDivElement} html DOM hosting the screen contents
     * @property {DataBinding} dataBinding Data binding helper
     * @property {boolean} dataBound Data binding helper
     * @property {string} search The search parameter
     */
    /**
     * @type {SceenContext}
     * */
    this._context = {
      html: this._html,
      dataBinding: this._dataBinding,
      dataBound: false,
      search: ''
    };

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
  }

  /**
   * allows to bind query results if any
   */
  async dataBindExecute() {
    // execute any scripts
    const script = this._html.querySelector("script");
    if (script && !this._context.dataBound) {
      this._dataBinding.executeInContext(script.innerText, this._context, true);
      this._context.dataBound = true;
      //if (this._context.promise)
        //await this._context.promise;
      //this._dataBinding.bindAll(this._html, this._context);
    }
    if (this._context.search)
      history.replaceState({ }, '',
        document.location.protocol + '//' + document.location.host +
        document.location.pathname + '?' + this._context.search + document.location.hash);
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