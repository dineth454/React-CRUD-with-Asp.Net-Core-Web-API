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

import { getUserRole } from "./Utils";

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [role, setRole] = useState(null);

  // Function to handle login
  const handleLogin = (token) => {
    // Store token in localStorage for persistence
    localStorage.setItem("access_token", token);
    setIsAuthenticated(true);

    const userRole = getUserRole(token);
    setRole(userRole);
    console.log('User role:', userRole);
  };

  return (
    <Provider store={store}>
      <ToastProvider autoDismiss={true}>
        <Container maxWidth="lg">
        {isAuthenticated ? (
          // Check for roles to determine what to render
          role === "Maker" ? (
            <DCandidates /> // Render admin-specific UI
          // ) : role === "Checker" ? (
          //   <UserDashboard /> // Render user-specific UI
          ) : (
            <p>Access Denied: You do not have the required permissions.</p>
          )
        ) : (
          <Login onLogin={handleLogin} /> // Render login page if not authenticated
        )}
        </Container>
      </ToastProvider>
    </Provider>
  );
}

export default App;

