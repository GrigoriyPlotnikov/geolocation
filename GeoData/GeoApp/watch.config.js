import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

import common from './webpack.config.js';
common.output.path = path.resolve(__dirname, '../bin/Debug/net5.0/wwwroot/js');

export default common;