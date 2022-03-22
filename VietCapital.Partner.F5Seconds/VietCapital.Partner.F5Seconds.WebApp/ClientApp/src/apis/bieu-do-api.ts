import {BieuDo, ResponseData} from '../models';
import axiosClient from './axiosClient';

const bieuDoApi = {
  getGDTheoNgay: (params: {ngayBatDau: any; ngayKetThuc: any}): Promise<ResponseData<BieuDo[]>> => {
    const url = '/bieuDo/giaodich-theongay';
    return axiosClient.get(url, {params});
  },
};

export default bieuDoApi;
