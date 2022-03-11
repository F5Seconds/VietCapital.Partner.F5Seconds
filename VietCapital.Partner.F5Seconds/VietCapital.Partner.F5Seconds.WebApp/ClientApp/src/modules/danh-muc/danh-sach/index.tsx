import {Button, IconButton, Stack} from '@mui/material';
import {Trash} from 'iconsax-react';
import {useSnackbar} from 'notistack';
import queryString from 'query-string';
import React, {useEffect, useState} from 'react';
import {useLocation, useNavigate} from 'react-router';
import {accountApi} from '../../../apis';
import {DataTable} from '../../../components/base';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {useWindowDimensions} from '../../../hooks';
import Header from '../../../layouts/Header';
import {Category, PaginationParams, QueryParams} from '../../../models';
import {categoryService} from '../../../services';
import {colors} from '../../../theme';

const DanhSachDanhMucPage = () => {
  const location = useLocation();
  const {enqueueSnackbar} = useSnackbar();
  const queryParams: QueryParams = queryString.parse(location.search);
  const navigate = useNavigate();
  const [openDialog, setOpenDialog] = useState<{open: boolean; id?: number | null}>({
    open: false,
    id: null,
  });
  const [isLoading, setIsLoading] = useState(false);
  const {height} = useWindowDimensions();
  const [isOpenDelete, setIsOpenDelete] = useState<{visible: boolean; id: number}>({
    visible: false,
    id: 0,
  });
  const [isDeleting, setIsDeleting] = useState(false);
  const [listCategory, setListCategory] = useState<Category[]>([]);
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

  const handleCloseDialog = () => setOpenDialog(prev => ({...prev, open: false}));

  const columns = [
    {field: 'image', headerName: 'Hình ảnh'},
    {
      field: 'name',
      headerName: 'Tên danh mục',
    },
    {
      field: 'status',
      headerName: 'Trạng thái',
    },
    {
      field: '',
      headerName: '',
      renderCell: (row: Category) => (
        <IconButton
          size="medium"
          color="error"
          onClick={e => {
            e.stopPropagation();
            setIsOpenDelete({visible: true, id: row.id});
          }}
        >
          <Trash color={colors.error} />
        </IconButton>
      ),
    },
  ];

  const handleSubmitUser = async (data: any) => {
    try {
      const res = await accountApi.register(data);
      if (res.succeeded) {
        enqueueSnackbar('Thêm mới user thành công', {variant: 'success'});
      } else {
        enqueueSnackbar(res.message, {variant: 'error'});
      }
      console.log(res);
    } catch (error) {
      console.log(error);
      enqueueSnackbar('Đã xảy ra lỗi', {variant: 'error'});
    }
  };
  const handleDelete = async () => {
    setIsDeleting(true);
    setIsOpenDelete(prev => ({...prev, open: false}));
    // const res = await accountService.deleteRole(isOpenDelete.id);
    // if (res) {
    //   // getAllRole();
    // }
    setIsDeleting(false);
  };
  useEffect(() => {
    const getList = async () => {
      const res = await categoryService.getAll(filters);
      if (res) {
        const {currentPage, pageSize, totalCount, totalPages, hasNext, hasPrevious} = res;
        setListCategory(res.data);
        setPagination({currentPage, pageSize, totalCount, totalPages, hasNext, hasPrevious});
      }
    };
    getList();
  });
  return (
    <div>
      <Header title="Danh sách danh mục" />
      <div style={{padding: 16}}>
        <Stack direction="row" justifyContent="flex-end" marginBottom={2}>
          <Button
            variant="contained"
            color="success"
            onClick={() => {
              navigate('them-danh-muc');
            }}
          >
            Thêm danh mục
          </Button>
        </Stack>
        <DataTable
          columns={columns}
          rows={listCategory}
          loading={isLoading}
          height={height - 200}
          onRowClick={row => {
            setOpenDialog(prev => ({...prev, open: true, id: row.id}));
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
              setFilters(prev => ({...prev, pageSize: value, pageNumber: 0}));
            },
          }}
        />
      </div>

      <LoadingOverlay open={isDeleting} />
    </div>
  );
};

export default DanhSachDanhMucPage;
