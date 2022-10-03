import { registerDeck } from "./navigator.js"


const app = async () => {
  registerDeck();
};

const loadIp = async () => {
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
        result.innerHTML = "Местоположение не найдено";
      else {
        result.innerHTML = `<div>IP: ${document.getElementById('ip').value}</div>`;
        result.innerHTML += `<div>City: ${location.city}</div>`;
      }
    });
}


document.addEventListener('DOMContentLoaded', app);