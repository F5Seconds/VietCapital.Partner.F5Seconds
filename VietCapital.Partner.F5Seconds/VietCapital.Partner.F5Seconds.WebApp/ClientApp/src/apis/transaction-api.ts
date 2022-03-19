import {Transaction, QueryParams, ResponseData, ResultData} from '../models';
import axiosClient from './axiosClient';

const billApi = {
  getAll: (params: QueryParams): Promise<ResponseData<ResultData<Transaction>>> => {
    const url = '/transaction';
    return axiosClient.get(url, {params});
  },
  getOne: (id: number | string): Promise<ResponseData<Transaction>> => {
    const url = `/transaction/${id}`;
    return axiosClient.get(url);
  },
  create: (data: Partial<Transaction>): Promise<ResponseData<Transaction>> => {
    const url = '/transaction';
    return axiosClient.post(url, data);
  },
  update: (id: number | string, data: Partial<Transaction>): Promise<ResponseData<number>> => {
    const url = `/transaction/${id}`;
    return axiosClient.put(url, data);
  },
  delete: (id: number | string): Promise<ResponseData<number>> => {
    const url = `/transaction/${id}`;
    return axiosClient.delete(url);
  },
};

export default billApi;
