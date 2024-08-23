import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import LoginPage from "../pages/LoginPage";
import HomePage from "../pages/HomePage";
import DashboardPage from "../pages/dashboard/DashboardPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "",
        element: <HomePage />,
      },
      {
        path: "login",
        element: <LoginPage />,
      },
      {
        path: "dashboard",
        element: <DashboardPage />,
      },
    ],
  },
]);
