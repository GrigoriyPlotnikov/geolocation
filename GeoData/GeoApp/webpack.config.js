import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

export default {
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
  },
  module: {
    rules: [
      {
        test: /\.m?js$/,
        exclude: /(node_modules|bower_components)/,
        use: {
          loader: 'babel-loader',
          options: {
            //presets: ['@babel/preset-env'],
            plugins: [
              "@babel/plugin-transform-react-jsx",
              "@babel/plugin-syntax-jsx"
            ]
          }
        }
      }
    ]
  }
};