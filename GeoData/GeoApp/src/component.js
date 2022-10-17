// @ts-check

//idea by Kartik Nair https://dev.to/kartiknair


/**
 * Allows jsx 
 * @param {string|Function} tag
 * @param {any} props
 * @param {...any} children
 * @returns {HTMLElement}
 */
export const createElement = (tag, props, ...children) => {
  if (typeof tag === "function") return tag(props, children)

  const element = document.createElement(tag);

  Object.entries(props || {}).forEach(([name, value]) => {
    if (name.startsWith("on") && name.toLowerCase() in window)
      element.addEventListener(name.toLowerCase().substr(2), value);
    else element.setAttribute(name, value.toString());
  });

  children.forEach(child => {
    appendChild(element, child);
  });

  return element;
}

  /**
   * 
   * 
   * @param {HTMLElement} parent
   * @param {Array|Node|string} child
   */
export const appendChild = (parent, child) => {
  if (Array.isArray(child))
    child.forEach(nestedChild => appendChild(parent, nestedChild));
  else if (child instanceof Node)
    parent.appendChild(child)
  else
    parent.appendChild(document.createTextNode(child));
}
