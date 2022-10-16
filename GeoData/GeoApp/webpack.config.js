// @ts-check

const path = require('path');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = (env, argv) => {
  return {
    mode: argv.mode === "production" ? "production" : "development",
    entry: ['./src/js/app.js'],
    output: {
      filename: 'app.js',
      path: path.resolve(__dirname, 'dist')
    },
    module: {
      rules: [
        {
          //test: /\.s[c|a]ss$/,
        }
      ]
    },
    devServer: {
      compress: true,
      port: 8083,
      //static: {
      //  directory: path.resolve(__dirname, 'GeoApp'),
      //},
      //contentBase: path.resolve(__dirname, 'GeoApp'),
      //publicPath: '/dist',
      open: false
    },
    plugins: [
      new CleanWebpackPlugin(),
    ]
  };
};