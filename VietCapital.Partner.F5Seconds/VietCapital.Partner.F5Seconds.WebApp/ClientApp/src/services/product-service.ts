import {productApi} from '../apis';
import {Category, Product, QueryParams, ResultData} from '../models';
import {setShowAlert} from '../redux/slice/alertSlice';
import store from '../redux/store';

const productService = {
  getAll: async (params: QueryParams): Promise<ResultData<Product> | undefined> => {
    try {
      const res = await productApi.getAll(params);
      if (res.succeeded) {
        return res.data;
      }
    } catch (error) {
      console.log('Lỗi get all Product');
    }
  },
  getOne: async (id: number | string): Promise<Product | undefined> => {
    try {
      const res = await productApi.getOne(id);
      if (res.succeeded) {
        return res.data;
      }
    } catch (error) {
      console.log('Lỗi get Product');
    }
  },
  create: async (data: Partial<Product>): Promise<Product | undefined> => {
    try {
      const res = await productApi.create(data);
      if (res.succeeded) {
        store.dispatch(setShowAlert({type: 'success', message: 'Thêm sản phẩm thành công'}));
      }
      return res.data;
    } catch (error) {
      store.dispatch(setShowAlert({type: 'error', message: 'Đã xảy ra lỗi'}));
    }
  },
  update: async (
    id: number | string,
    data: Partial<Product & {categoryProducts?: Category[]}>
  ): Promise<number | undefined> => {
    try {
      const res = await productApi.update(id, data);
      if (res.succeeded) {
        store.dispatch(setShowAlert({type: 'success', message: 'Cập nhật sản phẩm thành công'}));
      }
      return res.data;
    } catch (error) {
      store.dispatch(setShowAlert({type: 'error', message: 'Đã xảy ra lỗi'}));
    }
  },
  delete: async (id: number | string): Promise<number | undefined> => {
    try {
      const res = await productApi.delete(id);
      if (res.succeeded) {
        store.dispatch(setShowAlert({type: 'success', message: 'Xóa sản phẩm thành công'}));
      }
      return res.data;
    } catch (error) {
      store.dispatch(setShowAlert({type: 'error', message: 'Đã xảy ra lỗi'}));
    }
  },
};

export default productService;
