import axios from "axios";
import type { AxiosInstance } from "axios";

const client: AxiosInstance = axios.create({
  baseURL: "http://localhost:5122/api",
  timeout: 10000,
});

export default client;
