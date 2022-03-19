import {Button, CircularProgress, Grid, IconButton, Stack} from '@mui/material';
import {Box} from '@mui/system';
import {Trash} from 'iconsax-react';
import React, {FC, useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {DataTable, DialogBase} from '../../../../components/base';
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

const DialogPhanManHinh: FC<Props> = ({open = false, id, onClose, onSubmit}) => {
  const form = useForm({
    defaultValues: {
      role: null,
      user: [],
      claimName: null,
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
  const [listUsers, setListUsers] = useState([]);

  useEffect(() => {
    const getListRole = async () => {
      const res = await accountService.getAllRole();
      if (res) {
        setListRole(res);
      }
      setIsLoadingRole(false);
    };

    const getUsers = async () => {
      const res = await accountService.getAllUser();
      if (res) {
        setListUsers(res?.listUser);
      }
    };

    getListRole();
    getUsers();
  }, []);

  const role: any = watch('role');

  useEffect(() => {
    if (role) {
      accountService.getAllUsersByRole({roleId: role?.id}).then(res => {
        console.log('====================================');
        console.log(res);
        console.log('====================================');
        const list = res?.listUser;
        const users = listUsers
          ?.filter((item: any) => list?.includes(item?.username))
          .map((item: any) => ({...item, label: item?.name, value: item?.email}));

        setValue<any>('user', users);
      });
    }
  }, [role]);
  const claim: any = watch('claimName');
  useEffect(() => {
    setValue<any>('value', claim?.href);
  }, [claim]);

  const columns = [
    {
      field: 'claimName',
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
            // setIsOpenDelete({visible: true, id: row.id});
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
      onSubmit={handleSubmit(onSubmit)}
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
              name="claimName"
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
              <Button>Phân màn hình</Button>
            </Stack>
          </Grid>
          <Grid item xs={12}>
            <DataTable
              columns={columns}
              rows={[]}
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
      <LoadingOverlay open={isSubmitting} />
    </DialogBase>
  );
};

export default DialogPhanManHinh;
