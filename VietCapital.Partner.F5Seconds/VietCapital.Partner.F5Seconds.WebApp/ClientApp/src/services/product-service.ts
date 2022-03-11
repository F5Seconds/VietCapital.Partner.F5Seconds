import {productApi} from '../apis';
import {Product, QueryParams, ResultData} from '../models';

const productService = {
  getAll: async (params: QueryParams): Promise<ResultData<Product> | undefined> => {
    try {
      const res = await productApi.getAll(params);
      return res;
    } catch (error) {
      console.log('Lỗi get all product');
    }
  },
  getDetail: async (id: number | string): Promise<Product | undefined> => {
    try {
      const res = await productApi.getDetail(id);
      return res;
    } catch (error) {
      console.log('Lỗi get product');
    }
  },
};

export default productService;
