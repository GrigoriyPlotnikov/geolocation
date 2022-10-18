import { createElement } from '../component.js'
import { Location } from '../templates/location.js'

/** @jsx createElement */
export const Locations = (props) => (
  <div>
    <h1>Экран поиска списка метоположений</h1>
    <form onSubmit={formSubmit}>
      <label>
        Город:
        <input type="text" name='city' placeholder='cit_Gbqw4' />
      </label>
      <input type="submit" value="Искать" style="margin-left:8px" />
    </form>
    <div id="city-result" />
  </div>
);

export const checkCity = async function () {
  let city = new URLSearchParams(window.location.search).get('city');
  if (city)
    refreshState(city);
};

//provide behaviour to a from
async function formSubmit(event) {
  event.preventDefault();
  //replace the window link -- we are in next page
  let params = new URLSearchParams(window.location.search);
  params.set('city', this['city'].value);
  history.pushState({ 'city': this['city'].value }, '',
    document.location.protocol + '//' + document.location.host +
    document.location.pathname + '?' + params.toString() + document.location.hash);

  await refreshState(this['city'].value);
}

async function refreshState(city) {
  let results = await getData(ip);
  const res = document.getElementById("city-result");
  res.innerHTML = '';
  res.appendChild(Results(results));
}

/** @jsx createElement */
export const Results = (props) => (
  <div>
    <div><label>Результаты поиска по {props?.city} городу:</label> </div>
    {props?.locations?.length > 0
      ? <div className='results'>{props?.locations.map(location => <Location {...location} />)}</div>
      : <div />
    }
    {props?.errors?.length > 0
      ? <div>{props?.errors.map(error => (<div> <label>Ошибка: <span name="error">{error}</span></label></div>))}</div>
      : <div />
    }
  </div>
);

export const getData = async function (city) {
  const state = {
    city: city,
    errors: [],
    locations: []
  }
  await fetch(`./city/locations?city=${city}`)
    .then(response => {
      if (!response.ok) {
        throw new Error(`Сервер не смог обработать запрос. Код ответа: ${response.status}`);
      }
      return response.json();
    })
    .then((data) => {
      /** @type {GeoDataLocation[]} */
      const locations = data;
      if (!locations || !locations.length)
        throw new Error('Местоположение не найдено');
      state.locations.push(...locations);
    })
    .catch(ex => {
      state.errors.push(ex.message);
    });

  return state;
}