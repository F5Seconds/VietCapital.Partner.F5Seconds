import {Button, IconButton, Stack} from '@mui/material';
import {Trash} from 'iconsax-react';
import React, {useEffect, useState} from 'react';
import {DataTable, DialogConfirm} from '../../../components/base';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {useWindowDimensions} from '../../../hooks';
import Header from '../../../layouts/Header';
import {Role} from '../../../models';
import {accountService} from '../../../services';
import {colors} from '../../../theme';
import DialogRole from './dialog-role';

const PhanQuyenUser = () => {
  const [isLoading, setIsLoading] = useState(true);
  const {height} = useWindowDimensions();
  const [openDialog, setOpenDialog] = useState<{
    open: boolean;
    id?: string | null;
    roleName?: string;
  }>({
    open: false,
    id: null,
    roleName: '',
  });
  const [isOpenDelete, setIsOpenDelete] = useState({visible: false, id: ''});
  const [isDeleting, setIsDeleting] = useState(false);
  const [listRole, setListRole] = useState<Role[]>([]);
  const [filters, setFilters] = React.useState({
    // ...queryParams,
    // search: queryParams.search,
    // pageNumber: queryParams.pageNumber ?? 1,
    // pageSize: queryParams.pageSize ?? 10,
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
  const columns = [
    {field: 'id', headerName: 'ID'},
    {
      field: 'name',
      headerName: 'Tên quyền',
    },
    {
      field: '',
      headerName: '',
      renderCell: (row: Role) => (
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
  const handleCloseDialog = () => setOpenDialog(prev => ({...prev, open: false}));
  const handleSubmit = async (data: {roleName: string}) => {
    setIsDeleting(true);
    const res = openDialog.id
      ? await accountService.updateRole(openDialog.id, data.roleName)
      : await accountService.createRole(data.roleName);
    if (res) {
      setOpenDialog(prev => ({...prev, open: false}));
      getAllRole();
    }
    setIsDeleting(false);
  };
  const handleDelete = async () => {
    setIsDeleting(true);
    setOpenDialog(prev => ({...prev, open: false}));
    const res = await accountService.deleteRole(isOpenDelete.id);
    if (res) {
      getAllRole();
    }
    setIsDeleting(false);
  };
  const getAllRole = async () => {
    const res = await accountService.getAllRole();
    if (res) {
      setListRole(res);
    }
    setIsLoading(false);
  };
  useEffect(() => {
    getAllRole();
  }, []);
  return (
    <div>
      <Header title="Phân quyền user" />
      <div style={{padding: 16}}>
        <Stack direction="row" justifyContent="flex-end" marginBottom={2}>
          <Button
            variant="contained"
            color="info"
            sx={{marginRight: 1}}
            onClick={() => {
              setOpenDialog(prev => ({open: true}));
            }}
          >
            Gán quyền
          </Button>
          <Button
            variant="contained"
            color="success"
            onClick={() => {
              setOpenDialog(prev => ({open: true}));
            }}
          >
            Thêm quyền
          </Button>
        </Stack>
        <DataTable
          columns={columns}
          rows={listRole}
          loading={isLoading}
          height={height - 138}
          onRowClick={row => {
            setOpenDialog(prev => ({...prev, open: true, id: row.id, roleName: row.name}));
          }}
        />
      </div>
      {openDialog.open && (
        <DialogRole
          open={openDialog.open}
          id={openDialog.id}
          onClose={handleCloseDialog}
          onSubmit={handleSubmit}
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

export default PhanQuyenUser;
