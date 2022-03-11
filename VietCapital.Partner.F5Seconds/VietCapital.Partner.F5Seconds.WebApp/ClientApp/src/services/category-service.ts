import {categoryApi} from '../apis';
import {Category, QueryParams, ResultData} from '../models';

const categoryService = {
  getAll: async (params: QueryParams): Promise<ResultData<Category> | undefined> => {
    try {
      const res = await categoryApi.getAll(params);
      return res;
    } catch (error) {
      console.log('Lỗi get all category');
    }
  },
  getDetail: async (id: number | string): Promise<Category | undefined> => {
    try {
      const res = await categoryApi.getDetail(id);
      return res;
    } catch (error) {
      console.log('Lỗi get category');
    }
  },
};

export default categoryService;
