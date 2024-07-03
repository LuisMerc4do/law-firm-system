import React from "react";
import logo from "./logo.svg";
import "../src/styles/global.css";
import LoginPage from "./pages/LoginPage";
import { UserProvider } from "./context/useAuth";

function App() {
  return (
    <UserProvider>
      <div className="App">
        <header className="App-header">
          <LoginPage />
        </header>
      </div>
    </UserProvider>
  );
}

export default App;
