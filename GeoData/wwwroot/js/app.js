// initial code from JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/

import { registerDeck } from './navigator.js'
import { registerControls } from './controller.js'

const app = async () => {
  registerDeck();
  registerControls();
};

document.addEventListener('DOMContentLoaded', app);