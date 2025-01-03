import React, { useState } from "react";
import axios from "axios";

const Login = ({ onLogin }) => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  /*
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post("http://localhost:60671/connect/token", {
        grant_type: "password",
        username,
        password,
        client_id: "client",
        client_secret: "secret",
        scope: "api1"
      }
    );

      const token = response.data.access_token;
      onLogin(token); // Notify parent about successful login
    } catch (err) {
      setError("Invalid credentials. Please try again.");
    }
  };
*/

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
        const data = new URLSearchParams();
        data.append("grant_type", "password");
        data.append("username", username);
        data.append("password", password);
        data.append("client_id", "client");
        data.append("client_secret", "secret");

        const response = await axios.post("http://localhost:60671/connect/token", data, {
            headers: {
                "Content-Type": "application/x-www-form-urlencoded",
            },
        });

        const token = response.data.access_token;
        onLogin(token); // Notify parent about successful login
    } catch (err) {
        setError("Invalid credentials. Please try again.");
    }
};

  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Username:</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        {error && <p style={{ color: "red" }}>{error}</p>}
        <button type="submit">Login</button>
      </form>
    </div>
  );
};

export default Login;