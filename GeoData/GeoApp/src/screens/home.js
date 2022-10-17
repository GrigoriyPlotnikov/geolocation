﻿import { createElement } from '../component.js'

/** @jsx createElement */
export const Home = (props) => (
  <div>
    <h1>Экран информации о приложении</h1>
    <h2>Пожалуйста, выберите экран в меню слева.</h2>
    <ul>
      <li>Клиентская часть приложения. Выполнена в идеологии Single Page Application.</li>
      <li>Страница состоит из двух частей: в левой части меню переключения экранов, в правой части отображается выбранный экран.</li>
      <li>Клиентская часть реализовает два экрана: поиск гео-информации по IP, поиск списка местоположений по названию города.</li>
      <li>Экран поиска гео-информации содержит: поле для ввода IP адреса, кнопку &quot;Искать&quot; и область для вывода результата.</li>
      <li>По нажатию кнопки &quot;Искать&quot; на сервер отправляется запрос GET /ip/location?ip=123.234.123.234</li>
      <li>Обработанный ответ от сервера выводится в область вывода результатов.</li>
      <li>Экран поиска списка метоположений содержит: поле для ввода названия города, кнопку &quot;Искать&quot; и область для вывода результата.</li>
      <li>По нажатию кнопки &quot;Искать&quot; на сервер отправляется запрос GET /city/locations?city=cit_Gbqw4</li>
      <li>Обработанный ответ от сервера выводится в область вывода результатов.</li>
    </ul>
  </div>
);