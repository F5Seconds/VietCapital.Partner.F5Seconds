import {Category, QueryParams, ResultData} from '../models';
import axiosClient from './axiosClient';

const categoryApi = {
  getAll: (params: QueryParams): Promise<ResultData<Category>> => {
    const url = '/category/list';
    return axiosClient.get(url, {params});
  },
  getDetail: (id: number | string): Promise<Category> => {
    const url = `/category/detail?id=${id}`;
    return axiosClient.get(url);
  },
};

export default categoryApi;
