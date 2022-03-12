import React, {useEffect, useState} from 'react';
import Header from '../../../layouts/Header';
import {DataTable, DialogConfirm} from '../../../components/base';
import {useLocation, useNavigate} from 'react-router';
import queryString from 'query-string';
import {useWindowDimensions} from '../../../hooks';
import {Button, IconButton, Stack} from '@mui/material';
import DialogUser from './dialog-user';
import accountApi from '../../../apis/account-api';
import {useSnackbar} from 'notistack';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {Trash} from 'iconsax-react';
import {colors} from '../../../theme';
import {accountService} from '../../../services';
import {Account} from '../../../models';

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
  const [listUser, setListUser] = useState([]);
  const [isOpenDelete, setIsOpenDelete] = useState({visible: false, id: ''});
  const [isDeleting, setIsDeleting] = useState(false);
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
      field: 'id',
      headerName: 'Mã nhân viên',
    },
    {field: 'name', headerName: 'Tên nhân viên'},
    {field: 'username', headerName: 'Tên đăng nhập'},

    {
      field: 'email',
      headerName: 'Email',
    },
    {
      field: '',
      headerName: '',
      renderCell: (row: any) => (
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

  const handleSubmitUser = async (data: Account) => {
    setOpenDialog(prev => ({...prev, open: false}));
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
    setIsOpenDelete(prev => ({...prev, visible: false}));
    const res = await accountService.deleteRole(isOpenDelete.id);
    if (res) {
      getList();
    }
    setIsDeleting(false);
  };
  const getList = async () => {
    const res = await accountService.getAllUser();
    if (res) {
      console.log('====================================');
      console.log(res.listUser);
      console.log('====================================');
      setListUser(res.listUser);
    }
  };
  useEffect(() => {
    getList();
  }, []);
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
          rows={listUser}
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
      {openDialog.open && (
        <DialogUser
          open={openDialog.open}
          id={openDialog.id}
          onSubmit={handleSubmitUser}
          onClose={handleCloseDialog}
        />
      )}
      <DialogConfirm
        open={isOpenDelete.visible}
        title="Xác nhận"
        content='Bạn có chắc chắn muốn xóa quyền này"'
        onClose={() => setIsOpenDelete(prev => ({...prev, visible: false}))}
        onAgree={handleDelete}
      />
      <LoadingOverlay open={isDeleting} />
    </div>
  );
};

export default DanhSachUser;
