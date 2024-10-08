import React, { createContext, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { toast } from "react-hot-toast";
import { loginAPI, registerAPI } from "../services/AuthService";

export type UserProfileToken = {
  userName: string;
  email: string;
  token: string;
};

export type UserProfile = {
  userName: string;
  email: string;
};

type UserContextType = {
  user: UserProfile | null;
  token: string | null;
  registerUser: (
    email: string,
    username: string,
    password: string,
    firstName: string,
    lastName: string,
    phoneNumber: string
  ) => void;
  loginUser: (email: string, password: string) => void;
  logout: () => void;
  isLoggedIn: () => boolean;
};

const UserContext = createContext<UserContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error("useAuth must be used within a UserProvider");
  }
  return context;
};

type Props = { children: React.ReactNode };

export const UserProvider = ({ children }: Props) => {
  const navigate = useNavigate();
  const [token, setToken] = useState<string | null>(null);
  const [user, setUser] = useState<UserProfile | null>(null);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const storedUser = localStorage.getItem("user");
    const storedToken = localStorage.getItem("token");
    if (storedUser && storedToken) {
      setUser(JSON.parse(storedUser));
      setToken(storedToken);
      axios.defaults.headers.common["Authorization"] = `Bearer ${storedToken}`;
    }
    setIsReady(true);
  }, []);

  const registerUser = async (
    email: string,
    username: string,
    password: string,
    firstName: string,
    lastName: string,
    phoneNumber: string
  ) => {
    try {
      const res = await registerAPI(
        email,
        username,
        password,
        firstName,
        lastName,
        phoneNumber
      );
      if (res) {
        const userObj = {
          userName: res.userName,
          email: res.email,
        };
        localStorage.setItem("token", res.token);
        localStorage.setItem("user", JSON.stringify(userObj));
        setToken(res.token);
        setUser(userObj);
        toast.success("Register Success!");
        navigate("/search");
      }
    } catch (error) {
      toast.error("Error al crear la cuenta");
    }
  };

  const loginUser = async (email: string, password: string) => {
    try {
      const res = await loginAPI(email, password);
      if (res) {
        const userObj = {
          userName: res.userName,
          email: res.email,
        };
        localStorage.setItem("token", res.token);
        localStorage.setItem("user", JSON.stringify(userObj));
        setToken(res.token);
        setUser(userObj);
        navigate("/search");
      }
    } catch (error) {
      toast.error("Error al Iniciar Sesion");
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
    setToken(null);
    toast.success(
      "Te has desconectado de tu cuenta Exitosamente!, esperamos verte pronto."
    );
    navigate("/");
  };

  const isLoggedIn = () => !!user;

  return (
    <UserContext.Provider
      value={{ user, token, registerUser, loginUser, logout, isLoggedIn }}
    >
      {isReady ? children : null}
    </UserContext.Provider>
  );
};
