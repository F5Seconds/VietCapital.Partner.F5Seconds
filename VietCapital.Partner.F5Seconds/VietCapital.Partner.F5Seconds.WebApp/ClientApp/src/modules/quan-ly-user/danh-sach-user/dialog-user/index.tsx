import {CircularProgress, Grid, Stack} from '@mui/material';
import React, {FC, useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {DialogBase} from '../../../../components/base';
import LoadingOverlay from '../../../../components/base/loading-overlay';
import {InputField} from '../../../../components/hook-form';
import {accountService} from '../../../../services';

interface Props {
  open: boolean;
  id?: string | number | null;
  onClose: () => void;
  onSubmit: (data: any) => void;
}

const DialogUser: FC<Props> = ({open = false, id = null, onClose, onSubmit}) => {
  const form = useForm<any>({
    defaultValues: {
      lastName: '',
      firstName: '',
      email: '',
      username: '',
      // password: '',
      // confirmPassword: '',
    },
  });

  const {
    handleSubmit,
    getValues,
    formState: {isSubmitting},
  } = form;
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const getUser = async () => {
      const res = await accountService.getUserById(id);
      if (res) {
        // console.log(res);
        Object.keys(res).forEach((key: string) => {
          form.setValue(key, res[key]);
        });
        setIsLoading(false);
      }
    };
    id && getUser();
  }, [id]);
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
          {!id && (
            <>
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
            </>
          )}
        </Grid>
      )}
      <LoadingOverlay open={isSubmitting} />
    </DialogBase>
  );
};

export default DialogUser;
