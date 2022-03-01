import {useForm} from 'react-hook-form';
import React, {FC, useState} from 'react';
import {DialogBase} from '../../../../components/base';
import {CircularProgress, Grid, Stack} from '@mui/material';
import {InputField} from '../../../../components/hook-form';
import LoadingOverlay from '../../../../components/base/loading-overlay';

interface Props {
  open: boolean;
  id?: number | string;
  onClose: () => void;
  onSubmit: () => void;
}
const DialogUser: FC<Props> = ({open = false, id, onClose, onSubmit}) => {
  const form = useForm({
    defaultValues: {},
  });

  const {
    handleSubmit,
    formState: {isSubmitting},
  } = form;
  const [isLoading, setIsLoading] = useState(false);
  return (
    <DialogBase
      open={open}
      title={id ? 'Cập nhật bộ phận' : 'Tạo bộ phận'}
      onClose={onClose}
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
              name="username"
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
