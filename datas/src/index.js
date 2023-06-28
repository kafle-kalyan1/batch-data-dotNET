import React, { createContext } from 'react';
import ReactDOM from 'react-dom/client';

import App from './App';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
// import State from './Context/State';


const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>

    <App />
    <ToastContainer />

  </React.StrictMode>
);

