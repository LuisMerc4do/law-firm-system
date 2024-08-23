import axios from "axios";
import { UserProfileToken } from "../models/userModel";

const API_URL = "http://localhost:5224/api/v1";

// Add an interceptor to include the token in all requests
axios.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

const handleError = (error: any) => {
  if (axios.isAxiosError(error)) {
    console.error("Axios error:", error.response?.data);
    throw error.response?.data;
  } else {
    console.error("Unexpected error:", error);
    throw new Error("An unexpected error occurred");
  }
};

export const loginAPI = async (
  email: string,
  password: string
): Promise<UserProfileToken> => {
  try {
    const response = await axios.post<UserProfileToken>(`${API_URL}/login`, {
      email,
      password,
    });
    return response.data;
  } catch (error) {
    return handleError(error);
  }
};

export const registerAPI = async (
  email: string,
  username: string,
  password: string
): Promise<UserProfileToken> => {
  try {
    const response = await axios.post<UserProfileToken>(`${API_URL}/register`, {
      email,
      username,
      password,
    });
    return response.data;
  } catch (error) {
    return handleError(error);
  }
};

export const logoutAPI = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("user");
  delete axios.defaults.headers.common["Authorization"];
};

export const setAuthToken = (token: string) => {
  if (token) {
    axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  } else {
    delete axios.defaults.headers.common["Authorization"];
  }
};
