import axios, {AxiosRequestConfig, AxiosResponse} from 'axios';
import queryString from 'query-string';

const axiosClient = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json',
  },
  paramsSerializer: params => queryString.stringify(params),
});

axiosClient.interceptors.request.use(
  (config: AxiosRequestConfig | any) => {
    if (config.url?.indexOf('account') === -1) {
      config.baseURL = '/api/v1';
    }

    const token = localStorage.getItem('jwt');
    if (token) {
      if (config.headers) {
        config.headers.Authorization = `Bearer ${token}`;
      }
    }
    return config;
  },
  error => {
    // Do something with request error
    return Promise.reject(error);
  }
);

// Add a response interceptor
axiosClient.interceptors.response.use(
  (response: AxiosResponse) => {
    return response.data;
  },
  error => {
    return Promise.reject(error.response.data);
  }
);

export default axiosClient;
