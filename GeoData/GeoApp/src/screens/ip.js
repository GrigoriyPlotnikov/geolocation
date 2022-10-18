import { createElement } from '../component.js'
import { Location } from '../templates/location.js'

/** @jsx createElement */
export const Ip = (props) => (
  <div>
    <h1>Экран поиска гео-информации</h1>
    <form onSubmit={ formSubmit }>
      <label>
        Ip:
        <input type="text" name='ip' placeholder='123.234.123.234' />
      </label>
      <input type="submit" value="Искать" style="margin-left:8px" />
    </form>
    <div id="ip-result" />
  </div>
);

/** @jsx createElement */
export const Results = (props) => (
  <div>
    <div><label>Результаты поиска по {props?.ip} IP:</label> </div>
    {props?.locations?.length > 0
      ? <div>{props?.locations.map(location => <Location {...location} />)}</div>
      : <div />
    }
    {props?.errors?.length > 0
      ? <div>{props?.errors.map(error => (<div> <label>Ошибка: <span name="error">{error}</span></label></div>))}</div>
      : <div />
    }
  </div>
);

//provide behaviour to a from
async function formSubmit (event) {
  event.preventDefault();
  //replace the window link -- we are in next page
  let params = new URLSearchParams(window.location.search);
  params.set('ip', this['ip'].value);
  history.pushState({ 'ip': this['ip'].value }, '',
    document.location.protocol + '//' + document.location.host +
    document.location.pathname + '?' + params.toString() + document.location.hash);

  let results = await getData(this['ip'].value);
  const res = document.getElementById("ip-result");
  res.innerHTML = '';
  res.appendChild(Results(results));
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