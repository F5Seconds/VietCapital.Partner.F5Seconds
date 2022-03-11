import {Product, QueryParams, ResultData} from '../models';
import axiosClient from './axiosClient';

const productApi = {
  getAll: (params: QueryParams): Promise<ResultData<Product>> => {
    const url = '/product/list';
    return axiosClient.get(url, {params});
  },
  getDetail: (id: number | string): Promise<Product> => {
    const url = `/category/detail?id=${id}`;
    return axiosClient.get(url);
  },
};

export default productApi;
