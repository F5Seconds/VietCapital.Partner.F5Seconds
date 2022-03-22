import {Button, IconButton, Stack} from '@mui/material';
import {Trash} from 'iconsax-react';
import queryString from 'query-string';
import React, {useEffect, useState} from 'react';
import {useLocation, useNavigate} from 'react-router';
import {DataTable, DialogConfirm, SearchBar} from '../../../components/base';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {useWindowDimensions} from '../../../hooks';
import useCheckQuyen from '../../../hooks/useCheckQuyen';
import Header from '../../../layouts/Header';
import Page from '../../../layouts/Page';
import {PaginationParams, Product, QueryParams} from '../../../models';
import {productService} from '../../../services';
import {colors} from '../../../theme';

const DanhSachSanPhamPage = () => {
  const location = useLocation();
  const queryParams: any = queryString.parse(location.search);
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const {height} = useWindowDimensions();
  const [isOpenDelete, setIsOpenDelete] = useState<{visible: boolean; id: number | string}>({
    visible: false,
    id: 0,
  });
  const [isDeleting, setIsDeleting] = useState(false);
  const [listProduct, setListProduct] = useState<Product[]>([]);
  const [filters, setFilters] = useState<QueryParams>({
    ...queryParams,
    search: queryParams.search ?? '',
    pageNumber: queryParams.pageNumber ?? 1,
    pageSize: queryParams.pageSize ?? 10,
  });
  const [pagination, setPagination] = useState<PaginationParams>({
    currentPage: 1,
    totalPages: 1,
    pageSize: 10,
    totalCount: 0,
    hasPrevious: false,
    hasNext: false,
  });

  const columns = [
    {
      field: 'productCode',
      headerName: 'Mã sản phẩm',
    },
    {field: 'name', headerName: 'Tên sản phẩm'},

    {
      field: 'price',
      headerName: 'Giá',
    },
    {
      field: 'point',
      headerName: 'Điểm',
    },
    {
      field: 'partner',
      headerName: 'Đối tác',
    },
    {
      field: 'brandName',
      headerName: 'Thương hiệu',
    },
    // {
    //   field: '',
    //   headerName: '',
    //   renderCell: (row: any) => (
    //     <IconButton
    //       size="medium"
    //       color="error"
    //       onClick={e => {
    //         e.stopPropagation();
    //         setIsOpenDelete({visible: true, id: row.id});
    //       }}
    //     >
    //       <Trash color={colors.error} />
    //     </IconButton>
    //   ),
    // },
  ];

  const handleDelete = async () => {
    setIsDeleting(true);
    setIsOpenDelete(prev => ({...prev, visible: false}));
    const res = await productService.delete(isOpenDelete.id);
    if (res) {
      setFilters(prev => ({...prev, pageNumber: 1}));
    }
    setIsDeleting(false);
  };
  useEffect(() => {
    const getList = async () => {
      setIsLoading(true);
      const res = await productService.getAll(filters);
      if (res) {
        const {currentPage, pageSize, totalCount, totalPages, hasNext, hasPrevious} = res;

        setListProduct(res.data);
        setPagination({currentPage, pageSize, totalCount, totalPages, hasNext, hasPrevious});
      }
      setIsLoading(false);
    };
    getList();
  }, [filters]);
  const [checkQuyen] = useCheckQuyen();
  if (!checkQuyen('seen')) {
    navigate('/404');
  }
  return (
    <Page title="Danh sách sản phẩm">
      <Stack direction="row" justifyContent="space-between" marginBottom={2}>
        <SearchBar onSubmit={value => setFilters(prev => ({...prev, search: value}))} />

        {/* <Button
          variant="contained"
          onClick={() => {
            navigate('them-san-pham');
          }}
        >
          Thêm sản phẩm
        </Button> */}
      </Stack>
      <DataTable
        columns={columns}
        rows={listProduct}
        loading={isLoading}
        height={height - 200}
        onRowClick={(row: Product) => {
          navigate(`sua-san-pham/${row.id}`);
        }}
        pagination={{
          show: true,
          page: pagination.currentPage - 1,
          totalCount: pagination.totalCount,
          rowsPerPage: pagination.pageSize,
          onPageChange: page => {
            setFilters(prev => ({...prev, pageNumber: page + 1}));
          },
          onRowsPerPageChange: value => {
            setFilters(prev => ({...prev, pageSize: value, pageNumber: 1}));
          },
        }}
      />

      <DialogConfirm
        open={isOpenDelete.visible}
        title="Xác nhận"
        content="Bạn có chắc chắn muốn xóa sản phẩm này?"
        onClose={() => setIsOpenDelete(prev => ({...prev, visible: false}))}
        onAgree={handleDelete}
      />
      <LoadingOverlay open={isDeleting} />
    </Page>
  );
};

export default DanhSachSanPhamPage;
