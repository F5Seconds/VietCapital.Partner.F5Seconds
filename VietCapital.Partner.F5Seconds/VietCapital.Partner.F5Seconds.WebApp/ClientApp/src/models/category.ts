import {Product} from './product';

export interface Category {
  id: number;
  name: string;
  image: string;
  status: boolean;
  products: Product[];
}
