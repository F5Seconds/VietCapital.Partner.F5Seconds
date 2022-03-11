import {Category} from './category';

export interface Product {
  id: string;
  productCode: string;
  name: string;
  content: string;
  term: string;
  image: string;
  thumbnail: string;
  point: number;
  type: number;
  partner: string;
  brandName: string;
  brandLogo: string;
  categories: Category[];
}
