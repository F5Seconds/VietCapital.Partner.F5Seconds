import {Button, CircularProgress, Grid, IconButton, Stack} from '@mui/material';
import {RowHorizontal, Trash} from 'iconsax-react';
import {FC, useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {DataTable, DialogBase, DialogConfirm} from '../../../../components/base';
import LoadingOverlay from '../../../../components/base/loading-overlay';
import {AutocompleteAsyncField, InputField} from '../../../../components/hook-form';
import {items} from '../../../../layouts/Sidebar';
import {Role} from '../../../../models/role';
import {accountService} from '../../../../services';
import {colors} from '../../../../theme';

interface Props {
  open: boolean;
  id?: number | string | null;
  onClose: () => void;
  onSubmit: (data: any) => void;
}

const DialogPhanManHinh: FC<Props> = ({open = false, id, onClose}) => {
  const form = useForm({
    defaultValues: {
      role: null,
      claim: null,
      value: '',
    },
  });

  const {
    watch,
    handleSubmit,
    setValue,
    formState: {isSubmitting},
  } = form;
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingRole, setIsLoadingRole] = useState(true);
  const [listRole, setListRole] = useState<Role[]>([]);
  const [listClaim, setListClaim] = useState<any>([]);
  const [isOpenDelete, setIsOpenDelete] = useState<{visible: boolean; row: any}>({
    visible: false,
    row: null,
  });
  const [isDeleting, setIsDeleting] = useState(false);

  useEffect(() => {
    const getListRole = async () => {
      const res = await accountService.getAllRole();
      if (res) {
        setListRole(res);
      }
      setIsLoadingRole(false);
    };
    getListRole();
  }, []);

  const role: any = watch('role');

  const getClaim = () => {
    accountService.getAllClaimsInRole({roleName: role?.name}).then(res => {
      const list = res?.clams;
      list &&
        setListClaim(
          list.map(item => {
            const claim = items.find(i => i.href === item)?.title;
            return {
              claim,
              value: item,
            };
          })
        );
    });
  };
  useEffect(() => {
    if (role) {
      getClaim();
    }
  }, [role]);

  const claim: any = watch('claim');
  useEffect(() => {
    setValue<any>('value', claim?.href);
  }, [claim]);

  const columns = [
    {
      field: 'claim',
      headerName: 'Màn hình',
    },
    {
      field: 'value',
      headerName: 'URL của trang',
    },
    {
      field: '',
      headerName: '',
      renderCell: (row: any) => (
        <IconButton
          color="error"
          onClick={e => {
            e.stopPropagation();
            setIsOpenDelete({visible: true, row});
          }}
        >
          <Trash fontSize={20} color={colors.error} />
        </IconButton>
      ),
    },
  ];

  return (
    <DialogBase
      open={open}
      title="Phân màn hình"
      onClose={onClose}
      textPositive="Phân màn hình"
      onSubmit={() => {}}
      hasSubmitButton={false}
    >
      {isLoading ? (
        <Stack direction="row" justifyContent="center">
          <CircularProgress size={24} />
        </Stack>
      ) : (
        <Grid container spacing={2}>
          <Grid item xs={12}>
            <AutocompleteAsyncField
              form={form}
              name="role"
              label="Quyền *"
              items={listRole?.map(item => ({...item, label: item.name, value: item.name}))}
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
              loading={isLoadingRole}
            />
          </Grid>
          <Grid item xs={12}>
            <AutocompleteAsyncField
              form={form}
              name="claim"
              label="Tên màn hình *"
              items={items?.map(item => ({...item, label: item.title, value: item.href}))}
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
              loading={isLoadingRole}
            />
          </Grid>
          <Grid item xs={12}>
            <InputField
              disabled
              form={form}
              name="value"
              label="Url trang"
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
            />
          </Grid>
          <Grid item xs={12}>
            <Stack direction="row" justifyContent="flex-end">
              <Button variant="contained" onClick={handleSubmit(onSubmit)}>
                Phân màn hình
              </Button>
            </Stack>
          </Grid>
          <Grid item xs={12}>
            <DataTable
              columns={columns}
              rows={listClaim}
              loading={false}
              pagination={{
                show: false,
                page: 0,
                totalCount: 0,
                rowsPerPage: 0,
                onPageChange: page => {},
                onRowsPerPageChange: value => {},
              }}
            />
          </Grid>
        </Grid>
      )}
      <DialogConfirm
        open={isOpenDelete.visible}
        title="Xác nhận"
        content="Bạn có chắc chắn muốn xóa?"
        onClose={() => setIsOpenDelete(prev => ({...prev, visible: false}))}
        onAgree={handleDelete}
      />
      <LoadingOverlay open={isSubmitting} />
    </DialogBase>
  );
};

export default DialogPhanManHinh;
