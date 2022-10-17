const path = require('path');
const common = require('./webpack.config.js');

common.output.path = path.resolve(__dirname, '../bin/Debug/net5.0/wwwroot/js');
module.exports = common;