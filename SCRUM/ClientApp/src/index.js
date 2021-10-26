import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

import { CookiesProvider } from "react-cookie";

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <CookiesProvider>   
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>
  </CookiesProvider>   ,
  rootElement);

registerServiceWorker();

