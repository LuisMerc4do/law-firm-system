import axios from "axios";
import { UserProfileToken } from "../models/userModel";
import { handleError } from "./ErrorHandler";

const API_URL = "http://localhost:5224/api/v1";

export const loginAPI = async (email: string, password: string) => {
  try {
    const response = await axios.post(`${API_URL}/login`, {
      email,
      password,
    });
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.error("Axios error:", error.response?.data);
    } else {
      console.error("Unexpected error:", error);
    }
    throw error;
  }
};

export const registerAPI = async (
  email: string,
  username: string,
  password: string
) => {
  try {
    const response = await axios.post<UserProfileToken>(`${API_URL}/register`, {
      email,
      username,
      password,
    });
    return response.data;
  } catch (error) {
    handleError(error);
  }
};
