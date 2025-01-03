/*
import React from 'react';
import logo from './logo.svg';
import './App.css';
import { store } from "./actions/store";
import { Provider } from "react-redux";
import DCandidates from './components/DCandidates';
import { Container } from "@material-ui/core";
import { ToastProvider } from "react-toast-notifications";

function App() {
  return (
    <Provider store={store}>
      <ToastProvider autoDismiss={true}>
        <Container maxWidth="lg">
          <DCandidates />
        </Container>
      </ToastProvider>
    </Provider>
  );
}

export default App;
*/

import React, { useState } from 'react';
import './App.css';
import { store } from "./actions/store";
import { Provider } from "react-redux";
import DCandidates from './components/DCandidates';
import Login from './Login'; // Import your login page
import { Container } from "@material-ui/core";
import { ToastProvider } from "react-toast-notifications";

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  // Function to handle login
  const handleLogin = (token) => {
    // Store token in localStorage for persistence
    localStorage.setItem("access_token", token);
    setIsAuthenticated(true);
  };

  return (
    <Provider store={store}>
      <ToastProvider autoDismiss={true}>
        <Container maxWidth="lg">
          {isAuthenticated ? (
            <DCandidates /> // Render form page if authenticated
          ) : (
            <Login onLogin={handleLogin} /> // Render login page if not authenticated
          )}
        </Container>
      </ToastProvider>
    </Provider>
  );
}

export default App;

