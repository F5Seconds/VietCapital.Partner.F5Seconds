import {Product} from './product';

export interface Transaction {
  productId: number | string;
  transactionId: number | string;
  productPrice: number;
  customerId: string;
  customerPhone: string;
  voucherCode: string;
  state: number;
  expiryDate: Date;
  created: Date;
  product: Product;
}
