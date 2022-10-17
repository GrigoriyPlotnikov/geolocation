import { createElement } from '../component.js'

/** @jsx createElement */
export const Location = (props) => (
  <div className="loc">
    <div>Страна: <span name="country">{props.country}</span></div>
    <div>Регион: <span name="region">{props.region}</span></div>
    <div>Город: <span name="city">{props.city}</span></div>
    <div>Индекс: <span name="city">{props.postal}</span></div>
    <div>Организация: <br />&nbsp; &nbsp; <span name="city">{props.organization}</span></div>
    <div>Широта: <span name="city">{props.latitude}</span></div>
    <div>Долгота: <span name="city">{props.longitude}</span></div>
  </div>
);