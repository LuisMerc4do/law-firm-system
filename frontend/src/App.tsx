import "../src/styles/global.css";
import { UserProvider } from "./context/useAuth";
import { Outlet } from "react-router-dom";
import DefaultLayout from "./components/Layout/Layout";
import { Toaster } from "react-hot-toast";

function App() {
  return (
    <UserProvider>
      <DefaultLayout>
        <Outlet />
        <Toaster />
      </DefaultLayout>
    </UserProvider>
  );
}

export default App;
