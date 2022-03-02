export interface ResponseData {
  data: any;
  errors: boolean | null;
  succeeded: boolean;
  message: string;
}

export interface PaginationParams {
  page: number;
  pageSize: number;
  totalPage: number;
  search: string;
}
