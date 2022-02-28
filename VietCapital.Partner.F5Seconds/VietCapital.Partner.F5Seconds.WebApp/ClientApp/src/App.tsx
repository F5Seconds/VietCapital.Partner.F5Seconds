import React from 'react';
import {useRoutes} from 'react-router-dom';
import './App.css';
import {routes} from './routes';
function App() {
  return <div style={{minHeight: '100vh', backgroundColor: '#E3F1FD'}}>{useRoutes(routes)}</div>;
}

export default App;
