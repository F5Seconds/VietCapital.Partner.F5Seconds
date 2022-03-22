import {Grid} from '@mui/material';
import {FC} from 'react';
import {useForm} from 'react-hook-form';
import {DialogBase, LoadingOverLay} from '../../../components/base';
import {InputField} from '../../../components/hook-form';

interface Props {
  open: boolean;
  id?: string | number | null;
  onClose: () => void;
  onSubmit: (data: any) => void;
}

const DialogResetPassword: FC<Props> = ({open = false, id = null, onClose, onSubmit}) => {
  const form = useForm<any>({
    defaultValues: {
      password: '',
      confirmPassword: '',
    },
  });

  const {
    handleSubmit,
    getValues,
    formState: {isSubmitting},
  } = form;

  return (
    <DialogBase
      open={open}
      title={id ? 'Chỉnh sửa user' : 'Đổi mật khẩu'}
      onClose={onClose}
      textPositive={id ? 'Cập nhật' : 'Xác nhận'}
      onSubmit={handleSubmit(onSubmit)}
    >
      <Grid container spacing={2}>
        <Grid item xs={12} sm={6} xl={6}>
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
        <Grid item xs={12} sm={6} xl={6}>
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

      <LoadingOverLay open={isSubmitting} />
    </DialogBase>
  );
};

export default DialogResetPassword;
