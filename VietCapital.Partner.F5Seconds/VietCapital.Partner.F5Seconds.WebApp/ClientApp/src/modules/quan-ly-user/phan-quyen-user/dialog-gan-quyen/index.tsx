import {CircularProgress, Grid, Stack} from '@mui/material';
import React, {FC, useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {DialogBase} from '../../../../components/base';
import LoadingOverlay from '../../../../components/base/loading-overlay';
import {AutocompleteAsyncField} from '../../../../components/hook-form';
import {Role} from '../../../../models/role';
import {accountService} from '../../../../services';

interface Props {
  open: boolean;
  id?: number | string | null;
  onClose: () => void;
  onSubmit: (data: any) => void;
}

const DialogGanQuyen: FC<Props> = ({open = false, id, onClose, onSubmit}) => {
  const form = useForm({
    defaultValues: {
      role: null,
      user: [],
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
        // console.log(res);
        console.log('====================================');
        const list = res?.listUser;
        const users = listUsers
          ?.filter((item: any) => list?.includes(item?.username))
          .map((item: any) => ({...item, label: item?.name, value: item?.email}));

        setValue<any>('user', users);
      });
    }
  }, [role]);
  return (
    <DialogBase
      open={open}
      title="Gán quyền"
      onClose={onClose}
      textPositive="Gán quyền"
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
              name="user"
              label="Nhân viên *"
              items={listUsers?.map((item: any) => ({
                ...item,
                label: item.name,
                value: item.email,
              }))}
              multiple
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
              loading={isLoadingRole}
            />
          </Grid>
        </Grid>
      )}
      <LoadingOverlay open={isSubmitting} />
    </DialogBase>
  );
};

export default DialogGanQuyen;
