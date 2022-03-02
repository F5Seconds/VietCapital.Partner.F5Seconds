import React, {useState} from 'react';
import Header from '../../../layouts/Header';
import {DataTable} from '../../../components/base';
import {useLocation, useNavigate} from 'react-router';
import queryString from 'query-string';
import {useWindowDimensions} from '../../../hooks';
import {Button, Stack} from '@mui/material';
import DialogUser from './dialog-user';
import accountApi, {Account} from '../../../apis/account-api';
import {useSnackbar} from 'notistack';

const DanhSachUser = () => {
  const location = useLocation();
  const {enqueueSnackbar} = useSnackbar();
  const queryParams = queryString.parse(location.search);
  const navigate = useNavigate();
  const [openDialog, setOpenDialog] = useState<{open: boolean; id?: number | null}>({
    open: false,
    id: null,
  });
  const [isLoading, setIsLoading] = useState(false);
  const {height} = useWindowDimensions();

  const [filters, setFilters] = React.useState({
    ...queryParams,
    search: queryParams.search,
    pageNumber: queryParams.pageNumber ?? 1,
    pageSize: queryParams.pageSize ?? 10,
    tinhTrang: undefined,
  });
  const [pagination, setPagination] = useState({
    currentPage: 1,
    totalPages: 1,
    pageSize: 10,
    totalCount: 0,
    hasPrevious: false,
    hasNext: false,
  });

  const handleCloseDialog = () => setOpenDialog(prev => ({...prev, open: false}));

  const columns = [
    {
      field: 'maNs',
      headerName: 'Mã nhân sự',
      width: 90,
    },
    {field: 'hoTen', headerName: 'Người tạo yêu cầu', width: 130},

    {
      field: 'loaiYeuCau',
      headerName: 'Loại yêu cầu',
      width: 90,
    },
    {
      field: 'createdAt',
      headerName: 'Ngày tạo',
      width: 90,
    },
    {
      field: 'tdv',
      headerName: 'Trưởng đơn vị',

      width: 130,
    },
    {
      field: 'tienDo',
      headerName: 'Tiến độ',
      width: 90,
    },

    {
      field: 'tinhTrang',
      headerName: 'Tình trạng',
      width: 130,
    },
    {
      field: '',
      headerName: '',
      width: 160,
    },
  ];

  const handleSubmitUser = async (data: Account) => {
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
      //   enqueueSnackbar(error?.message, {variant: 'error'});
    }
  };
  return (
    <div>
      <Header title="Danh sách user" />
      <div style={{padding: 16}}>
        <Stack direction="row" justifyContent="flex-end" marginBottom={2}>
          <Button
            variant="contained"
            color="success"
            onClick={() => {
              setOpenDialog(prev => ({open: true}));
            }}
          >
            Thêm user
          </Button>
        </Stack>
        <DataTable
          columns={columns}
          rows={[]}
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
      <DialogUser
        open={openDialog.open}
        id={openDialog.id}
        onSubmit={handleSubmitUser}
        onClose={handleCloseDialog}
      />
    </div>
  );
};

export default DanhSachUser;
