import { createElement } from '../src/component.js'

it('renders without crashing', async () => {
  const div = document.createElement('div');
  /** @jsx createElement */
  const Subj = () => (
    <span />
  );
  div.appendChild(<Subj />);
  await new Promise(resolve => setTimeout(resolve, 1000));
});