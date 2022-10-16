const path = require('path');

module.exports = {
  entry: './src/app.js',
  resolve: {
    extensions: ['.js'],
  },
  output: {
    //library: {
    //  name: 'MYAPP',
    //  type: 'var'
    //},
    filename: 'app.js',
    path: path.resolve(__dirname, '../wwwroot/js'),
  }
};