export interface ResponseData<T> {
  data: T;
  errors: boolean | null;
  succeeded: boolean;
  message: string;
}

export interface PaginationParams {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface QueryParams {
  search?: string;
  pageNumber?: number | string;
  pageSize?: number | string;
}

export interface ResultData<T> extends PaginationParams {
  data: T[];
}
