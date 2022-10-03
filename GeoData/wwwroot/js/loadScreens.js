///initial code from JeremyLikness https://github.com/JeremyLikness/vanillajs-deck/

//@ts-check
import { Screen } from "./screen.js"

/**
 * Load a single screen
 * @param {string} screenName The name 
 * @returns {Promise<Screen>} The screen 
 */
async function loadScreen(screenName) {
  const response = await fetch(`./screens/${screenName}.html`);
  const screen = await response.text();
  return new Screen(screen);
}

export async function loadScreens() {
  const slides = {}
  slides['home'] = await loadScreen('home');
  slides['ip'] = await loadScreen('home');
  slides['locations'] = await loadScreen('home');
  return slides;
}