import {Product, QueryParams, ResponseData, ResultData} from '../models';
import axiosClient from './axiosClient';

const productApi = {
  getAll: (params: QueryParams): Promise<ResponseData<ResultData<Product>>> => {
    const url = '/product';
    return axiosClient.get(url, {params});
  },
  getOne: (id: number | string): Promise<ResponseData<Product>> => {
    const url = `/product/${id}`;
    return axiosClient.get(url);
  },
  create: (data: Partial<Product>): Promise<ResponseData<Product>> => {
    const url = '/product';
    return axiosClient.post(url, data);
  },
  update: (id: number | string, data: Partial<Product>): Promise<ResponseData<number>> => {
    const url = `/product/${id}`;
    return axiosClient.put(url, data);
  },
  delete: (id: number | string): Promise<ResponseData<number>> => {
    const url = `/product/${id}`;
    return axiosClient.delete(url);
  },
};

export default productApi;
