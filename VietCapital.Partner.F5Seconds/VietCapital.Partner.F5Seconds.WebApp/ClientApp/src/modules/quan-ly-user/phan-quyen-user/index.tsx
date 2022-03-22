import {Button, IconButton, Stack} from '@mui/material';
import {Trash} from 'iconsax-react';
import React, {useEffect, useState} from 'react';
import {useNavigate} from 'react-router-dom';
import {DataTable, DialogConfirm} from '../../../components/base';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {useWindowDimensions} from '../../../hooks';
import useCheckQuyen from '../../../hooks/useCheckQuyen';
import Page from '../../../layouts/Page';
import {Role} from '../../../models';
import {accountService} from '../../../services';
import {colors} from '../../../theme';
import DialogGanQuyen from './dialog-gan-quyen';
import DialogRole from './dialog-role';

const PhanQuyenUser = () => {
  const navigate = useNavigate();
  const [checkQuyen] = useCheckQuyen();
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
  const [openDialogGanQuyen, setOpenDialogGanQuyen] = useState<{
    open: boolean;
    id?: string | null;
  }>({
    open: false,
    id: null,
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
    ...(checkQuyen('delete')
      ? [
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
        ]
      : []),
  ];

  const handleCloseDialog = () => setOpenDialog(prev => ({...prev, open: false}));
  const handleCloseDialogGanQuyen = () => setOpenDialogGanQuyen(prev => ({...prev, open: false}));

  const handleSubmit = async (data: {roleName: string}) => {
    setIsDeleting(true);
    setOpenDialog(prev => ({...prev, open: false}));
    const res = openDialog.id
      ? await accountService.updateRole(openDialog.id, data.roleName)
      : await accountService.createRole(data.roleName);
    if (res) {
      getAllRole();
    }
    setIsDeleting(false);
  };
  const handleSubmitGanQuyen = async (data: any) => {
    setIsDeleting(true);
    setOpenDialogGanQuyen(prev => ({...prev, open: false}));
    const newData = data?.user?.map((item: any) => ({
      maNhanVien: item?.id,
      tenNhanVien: item?.name,
      email: item?.email,
    }));
    const res = await accountService.addUsersToRole({roleName: data?.role?.name}, newData);
    if (res) {
      getAllRole();
    }
    // console.log(data);
    setIsDeleting(false);
  };
  const handleDelete = async () => {
    setIsDeleting(true);
    setIsOpenDelete(prev => ({...prev, visible: false}));
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

  if (!checkQuyen('seen')) {
    navigate('/404');
  }
  return (
    <Page title="Phân quyền user">
      <Stack direction="row" justifyContent="flex-end" marginBottom={2}>
        {/* <Button
          variant="contained"
          color="primary"
          sx={{marginRight: 1}}
          onClick={() => {
            setOpenDialogPhanManHinh(prev => ({open: true}));
          }}
        >
          Phân màn hình
        </Button> */}
        {checkQuyen('edit') && (
          <Button
            variant="contained"
            color="primary"
            sx={{marginRight: 1}}
            onClick={() => {
              setOpenDialogGanQuyen(prev => ({open: true}));
            }}
          >
            Gán quyền
          </Button>
        )}

        {checkQuyen('create') && (
          <Button
            variant="contained"
            onClick={() => {
              setOpenDialog(prev => ({open: true}));
            }}
          >
            Thêm quyền
          </Button>
        )}
      </Stack>
      <DataTable
        columns={columns}
        rows={listRole}
        loading={isLoading}
        height={height - 138}
        onRowClick={row => {
          checkQuyen('edit') &&
            setOpenDialog(prev => ({...prev, open: true, id: row.id, roleName: row.name}));
        }}
      />

      {openDialog.open && (
        <DialogRole
          open={openDialog.open}
          id={openDialog.id}
          roleName={openDialog.roleName}
          onClose={handleCloseDialog}
          onSubmit={handleSubmit}
        />
      )}
      {openDialogGanQuyen.open && (
        <DialogGanQuyen
          open={openDialogGanQuyen.open}
          id={openDialogGanQuyen.id}
          onClose={handleCloseDialogGanQuyen}
          onSubmit={handleSubmitGanQuyen}
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
    </Page>
  );
};

export default PhanQuyenUser;
