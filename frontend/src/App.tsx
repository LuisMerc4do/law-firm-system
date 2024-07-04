import React from "react";
import logo from "./logo.svg";
import "../src/styles/global.css";
import LoginPage from "./pages/LoginPage";
import { UserProvider } from "./context/useAuth";
import { ToastContainer } from "react-toastify";
import { Outlet } from "react-router-dom";
import DefaultLayout from "./components/Layout/Layout";

function App() {
  return (
    <UserProvider>
      <DefaultLayout>
        <Outlet />
        <ToastContainer />
      </DefaultLayout>
    </UserProvider>
  );
}

export default App;
