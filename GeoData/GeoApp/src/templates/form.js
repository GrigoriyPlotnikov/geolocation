import { createElement } from '../component.js'

/** @jsx createElement */
export const Form = (props) => (
  <form>
    <label>
      {props?.label ? props.label : 'label'}
      <input type="text" name={props?.name ? props.name : 'name'} placeholder={props?.placeholder ? props.placeholder : 'placeholder'} />
    </label>
    <input type="submit" value="Искать" style="margin-left:8px" />
  </form>
);