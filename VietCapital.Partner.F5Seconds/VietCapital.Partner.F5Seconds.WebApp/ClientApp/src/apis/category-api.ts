import {Category, QueryParams, ResponseData, ResultData} from '../models';
import axiosClient from './axiosClient';

const categoryApi = {
  getAll: (params: QueryParams): Promise<ResponseData<ResultData<Category>>> => {
    const url = '/category';
    return axiosClient.get(url, {params});
  },
  getOne: (id: number | string): Promise<ResponseData<Category>> => {
    const url = `/category/${id}`;
    return axiosClient.get(url);
  },
  create: (data: Partial<Category>): Promise<ResponseData<Category>> => {
    const url = '/category';
    return axiosClient.post(url, data);
  },
  update: (id: number | string, data: Partial<Category>): Promise<ResponseData<number>> => {
    const url = `/category/${id}`;
    return axiosClient.put(url, data);
  },
  delete: (id: number | string): Promise<ResponseData<number>> => {
    const url = `/category/${id}`;
    return axiosClient.delete(url);
  },
};

export default categoryApi;
