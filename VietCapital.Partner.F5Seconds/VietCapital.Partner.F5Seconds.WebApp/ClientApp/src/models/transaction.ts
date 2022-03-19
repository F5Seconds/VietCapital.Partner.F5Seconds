import {Product} from './product';

export interface Transaction {
  productId: number | string;
  productIdtransactionId: number | string;
  productPrice: number;
  customerId: string;
  customerPhone: string;
  voucherCode: string;
  state: number;
  expiryDate: Date;
  product: Product;
}
