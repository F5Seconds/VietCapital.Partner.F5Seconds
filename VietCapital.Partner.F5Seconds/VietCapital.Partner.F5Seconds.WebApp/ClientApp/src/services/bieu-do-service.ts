import moment from 'moment';
import bieuDoApi from '../apis/bieu-do-api';
import {BieuDo} from '../models';

const bieuDoService = {
  getGDTheoNgay: async (params: {
    ngayBatDau: Date;
    ngayKetThuc: Date;
  }): Promise<BieuDo[] | undefined> => {
    try {
      const res = await bieuDoApi.getGDTheoNgay({
        ngayBatDau: moment(params.ngayBatDau).format('YYYY-MM-DD'),
        ngayKetThuc: moment(params.ngayKetThuc).format('YYYY-MM-DD'),
      });
      if (res.succeeded) {
        return res.data;
      }
    } catch (error) {
      console.log('Lỗi get all category');
    }
  },
};

export default bieuDoService;
