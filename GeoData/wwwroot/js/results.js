// @ts-check

/**
 * Custom element that renders search results
 * @extends {HTMLElement}
 */
export class Results extends HTMLElement {
  constructor() {
    super()


  }

  /**
   * Called when the element is inserted into the DOM. Used to fetch the template and wire into the related Navigator instance.
   */
  async connectedCallback() {
    //subscribe to form submit
    document.querySelector('form').addEventListener('submit', (ev) => {
      ev.preventDefault();
    });
  }

  loadIp() {
    fetch(`./ip/location?ip=${document.getElementById('ip').value}`)
      .then(response => {
        if (response.ok) {
          return response.json;
        }
        if (res)
          throw new Error('');
      })
      .then(json => {
        let location = JSON.parse(json);
        let result = document.getElementById('result');
        if (Object.keys(location).length === 0)
          result.innerHTML = 'Местоположение не найдено';
        else {
          result.innerHTML = `<div>IP: ${document.getElementById('ip').value}</div>`;
          result.innerHTML += `<div>City: ${location.city}</div>`;
        }
      });
  }
}

/**
 * Register the custom screen-deck component
 */
export const registerResults = () => customElements.define('results', Results);