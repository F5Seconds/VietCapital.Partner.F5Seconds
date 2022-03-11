import {categoryApi} from '../apis';
import {Category, QueryParams, ResultData} from '../models';
import {setShowAlert} from '../redux/slice/alertSlice';
import store from '../redux/store';

const categoryService = {
  getAll: async (params: QueryParams): Promise<ResultData<Category> | undefined> => {
    try {
      const res = await categoryApi.getAll(params);
      if (res.succeeded) {
        return res.data;
      }
    } catch (error) {
      console.log('Lỗi get all category');
    }
  },
  getOne: async (id: number | string): Promise<Category | undefined> => {
    try {
      const res = await categoryApi.getOne(id);
      if (res.succeeded) {
        return res.data;
      }
    } catch (error) {
      console.log('Lỗi get category');
    }
  },
  create: async (data: Partial<Category>): Promise<Category | undefined> => {
    try {
      const res = await categoryApi.create(data);
      if (res.succeeded) {
        store.dispatch(setShowAlert({type: 'success', message: 'Thêm danh mục thành công'}));
      }
      return res.data;
    } catch (error) {
      store.dispatch(setShowAlert({type: 'error', message: 'Đã xảy ra lỗi'}));
    }
  },
  update: async (id: number | string, data: Partial<Category>): Promise<number | undefined> => {
    try {
      const res = await categoryApi.update(id, data);
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
      const res = await categoryApi.delete(id);
      if (res.succeeded) {
        store.dispatch(setShowAlert({type: 'success', message: 'Xóa danh mục thành công'}));
      }
      return res.data;
    } catch (error) {
      store.dispatch(setShowAlert({type: 'error', message: 'Đã xảy ra lỗi'}));
    }
  },
};

export default categoryService;
