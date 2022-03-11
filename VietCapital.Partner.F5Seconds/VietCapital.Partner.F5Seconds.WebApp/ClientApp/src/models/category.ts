import {Product} from './product';

export interface Category {
  id: number | string;
  name: string;
  image: string;
  status: boolean;
  products: Product[];
}
