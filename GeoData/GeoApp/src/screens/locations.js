import { createElement } from '../component.js'
import { Form } from '../templates/form.js'
import { Location } from '../templates/location.js'

/** @jsx createElement */
export const Locations = (props) => (
  <div>
    <h1>Экран поиска списка метоположений</h1>
    <Form label='Город:' name='city' placeholder="cit_Gbqw4" />
    <div><label>Результаты поиска по {props?.city} городу:</label> </div>
    <div className="results">{props?.locations.map(location => <Location {...location} />)}</div>
    <div>{props?.errors.map(error => (<div> <label>Ошибка: <span name="error">{ error }</span></label></div>)) }</div>
  </div>
);