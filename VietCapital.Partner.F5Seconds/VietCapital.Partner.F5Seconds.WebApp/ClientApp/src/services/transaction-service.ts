import {transactionApi} from '../apis';
import {Transaction, QueryParams, ResultData} from '../models';
import {setShowAlert} from '../redux/slice/alertSlice';
import store from '../redux/store';

const transactionService = {
  getAll: async (params: QueryParams): Promise<ResultData<Transaction> | undefined> => {
    try {
      const res = await transactionApi.getAll(params);
      if (res.succeeded) {
        return res.data;
      }
    } catch (error) {
      console.log('Lỗi get all category');
    }
  },
  getOne: async (id: number | string): Promise<Transaction | undefined> => {
    try {
      const res = await transactionApi.getOne(id);
      if (res.succeeded) {
        return res.data;
      }
    } catch (error) {
      console.log('Lỗi get category');
    }
  },
  create: async (data: Partial<Transaction>): Promise<Transaction | undefined> => {
    try {
      const res = await transactionApi.create(data);
      if (res.succeeded) {
        store.dispatch(setShowAlert({type: 'success', message: 'Thêm danh mục thành công'}));
      }
      return res.data;
    } catch (error) {
      store.dispatch(setShowAlert({type: 'error', message: 'Đã xảy ra lỗi'}));
    }
  },
  update: async (id: number | string, data: Partial<Transaction>): Promise<number | undefined> => {
    try {
      const res = await transactionApi.update(id, data);
      if (res.succeeded) {
        store.dispatch(setShowAlert({type: 'success', message: 'Cập nhật danh mục thành công'}));
      }
      return res.data;
    } catch (error) {
      store.dispatch(setShowAlert({type: 'error', message: 'Đã xảy ra lỗi'}));
    }
  },
  delete: async (id: number | string): Promise<number | undefined> => {
    try {
      const res = await transactionApi.delete(id);
      if (res.succeeded) {
        store.dispatch(setShowAlert({type: 'success', message: 'Xóa danh mục thành công'}));
      }
      return res.data;
    } catch (error) {
      store.dispatch(setShowAlert({type: 'error', message: 'Đã xảy ra lỗi'}));
    }
  },
};

export default transactionService;
