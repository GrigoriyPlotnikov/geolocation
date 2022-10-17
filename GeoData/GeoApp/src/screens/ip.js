import { createElement } from '../component.js'
import { Form } from '../templates/form.js'
import { Location } from '../templates/location.js'

/** @jsx createElement */
export const Ip = (props, state) => (
  <div>
    <h1>Экран поиска гео-информации</h1>
    <Form onSubmit={formSubmit} label='Ip:' name='ip' placeholder="123.234.123.234" />
    <div><label>Результаты поиска по {state?.ip} IP:</label> </div>
    <div>{state?.locations.map(location => <Location {...location} />)}</div>
    <div>{state?.errors.map(error => (<div> <label>Ошибка: <span name="error">{error}</span></label></div>))}</div>
  </div>
);

//provide behaviour to a from
async function formSubmit (event) {
  event.preventDefault();
  alert(this);
  return;
  ////replace the window link -- we are in next page
  //let params = new URLSearchParams(window.location.search);
  //params.set('ip', this['ip'].value);
  //history.pushState({ 'ip': this['ip'].value }, '',
  //  document.location.protocol + '//' + document.location.host +
  //  document.location.pathname + '?' + params.toString() + document.location.hash);
  ////provide values for observer
  //state.ip.value = this['ip'].value;
  //state.locations.length = 0;
  //state.errors.length = 0;
  //state.search = params.toString();
  ////perform the request and see what is there
  //getData(this['ip'].value, state);
}

export const getData = async function (ip) {
  const state = {
    ip: ip,
    errors: [],
    locations: []
  }
  await fetch(`./ip/location?ip=${ip}`)
    .then(response => {
      if (response.status === 404) {
        throw new Error('Местоположение не найдено');
      }
      if (!response.ok) {
        throw new Error(`Сервер не смог обработать запрос. Код ответа: ${response.status}`);
      }
      return response.json();
    })
    .then((data) => {
      /** @type {GeoDataLocation} */
      const loc = data;
      if (!loc || !loc.city)
        throw new Error('Местоположение не найдено');
      state.locations.push(loc);
    })
    .catch(ex => {
      state.errors.push(ex.message);
    });

  return state;
}