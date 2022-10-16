// initial code from JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/

import { registerDeck } from './navigator.js'
import { registerControls } from './controls.js'
import { createElement } from './component.js'

const app = async () => {

	/** @jsx createElement */
	const Root = () => (
		<div id="root">
			<screen-controls deck="main"> --- </screen-controls>
			<screen-deck id="main" start="home">
				<h1>Клиентская часть приложения. Выполнена в идеологии Single Page Application.</h1>
				<h2>Идёт загрузка ...</h2>
			</screen-deck>
		</div>
	);

	document.getElementsByTagName('body')[0].appendChild(<Root />);

	registerDeck();
	registerControls();
};

document.addEventListener('DOMContentLoaded', app);