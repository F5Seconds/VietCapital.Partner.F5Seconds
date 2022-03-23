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
      oldPassword: '',
      newPassword: '',
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
        <Grid item xs={12}>
          <InputField
            form={form}
            name="oldPassword"
            label="Mật khẩu hiện tại"
            type="password"
            rules={{
              required: {
                value: true,
                message: 'Không được để trống',
              },
            }}
          />
        </Grid>
        <Grid item xs={12}>
          <InputField
            form={form}
            name="newPassword"
            label="Mật khẩu mới"
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
        <Grid item xs={12}>
          <InputField
            form={form}
            name="confirmPassword"
            label="Nhập lại mật khẩu mới"
            type="password"
            rules={{
              required: {
                value: true,
                message: 'Không được để trống',
              },
              validate: {
                match: (value: string) => {
                  const {newPassword} = getValues();
                  return value === newPassword || 'Mật khẩu không khớp.';
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
