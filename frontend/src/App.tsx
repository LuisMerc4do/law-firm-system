import "../src/styles/global.css";
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
