import {CircularProgress, Grid, Stack} from '@mui/material';
import React, {FC, useState} from 'react';
import {useForm} from 'react-hook-form';
import {DialogBase} from '../../../../components/base';
import LoadingOverlay from '../../../../components/base/loading-overlay';
import {InputField} from '../../../../components/hook-form';

interface Props {
  open: boolean;
  id?: number | string | null;
  onClose: () => void;
  onSubmit: (data: any) => void;
}

const DialogUser: FC<Props> = ({open = false, id, onClose, onSubmit}) => {
  const form = useForm({
    defaultValues: {
      lastName: '',
      firstName: '',
      email: '',
      userName: '',
      password: '',
      confirmPassword: '',
    },
  });

  const {
    handleSubmit,
    getValues,
    formState: {isSubmitting},
  } = form;
  const [isLoading, setIsLoading] = useState(false);

  return (
    <DialogBase
      open={open}
      title={id ? 'Chỉnh sửa user' : 'Tạo user mới'}
      onClose={onClose}
      textPositive={id ? 'Cập nhật' : 'Tạo'}
      onSubmit={handleSubmit(onSubmit)}
    >
      {isLoading ? (
        <Stack direction="row" justifyContent="center">
          <CircularProgress size={24} />
        </Stack>
      ) : (
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6} xl={4}>
            <InputField
              form={form}
              name="lastName"
              label="Họ"
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
            />
          </Grid>
          <Grid item xs={12} sm={6} xl={4}>
            <InputField
              form={form}
              name="firstName"
              label="Tên"
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
            />
          </Grid>
          <Grid item xs={12} sm={6} xl={4}>
            <InputField
              form={form}
              name="email"
              label="Email"
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
            />
          </Grid>
          <Grid item xs={12} sm={6} xl={4}>
            <InputField
              form={form}
              name="userName"
              label="Tên đăng nhập"
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
            />
          </Grid>
          <Grid item xs={12} sm={6} xl={4}>
            <InputField
              form={form}
              name="password"
              label="Mật khẩu"
              type="password"
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
                validate: {
                  match: (value: string) => {
                    const {confirmPassword} = getValues();
                    return value === confirmPassword || 'Mật khẩu không khớp.';
                  },
                },
              }}
            />
          </Grid>
          <Grid item xs={12} sm={6} xl={4}>
            <InputField
              form={form}
              name="confirmPassword"
              label="Nhập lại mật khẩu"
              type="password"
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
                validate: {
                  match: (value: string) => {
                    const {password} = getValues();
                    return value === password || 'Mật khẩu không khớp.';
                  },
                },
              }}
            />
          </Grid>
        </Grid>
      )}
      <LoadingOverlay open={isSubmitting} />
    </DialogBase>
  );
};

export default DialogUser;
