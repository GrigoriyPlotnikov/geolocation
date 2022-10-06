//@ts-check

// initial code from JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/

import { Screen } from './screen.js'
import { DataBinding } from "./dataBinding.js"

/**
 * Load a single screen
 * @param {string} screenRoute The name
 * @returns {Promise<Screen>} The screen 
 */
async function loadScreen(screenRoute) {
  const response = await fetch(`./screens/${screenRoute}.html`);
  const screen = await response.text();
  const s = new Screen(screen, screenRoute);
  await s.dataBindExecute();
  return s;
}

/**
 * @returns {Promise<Screen[]>} Loaded screens
 */
export async function loadScreens() {
  const slides = []
  slides.push(await loadScreen('home'));
  slides.push(await loadScreen('ip'));
  slides.push(await loadScreen('locations'));
  return slides;
}