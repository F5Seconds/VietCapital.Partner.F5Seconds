import {CircularProgress, Grid, Stack} from '@mui/material';
import React, {FC, useState} from 'react';
import {useForm} from 'react-hook-form';
import {DialogBase} from '../../../../components/base';
import LoadingOverlay from '../../../../components/base/loading-overlay';
import {InputField} from '../../../../components/hook-form';

interface Props {
  open: boolean;
  id?: number | string | null;
  roleName?: string;
  onClose: () => void;
  onSubmit: (data: any) => void;
}

const DialogRole: FC<Props> = ({open = false, id, roleName = '', onClose, onSubmit}) => {
  const form = useForm({
    defaultValues: {
      roleName,
    },
  });

  const {
    handleSubmit,
    formState: {isSubmitting},
  } = form;
  const [isLoading, setIsLoading] = useState(false);

  return (
    <DialogBase
      open={open}
      title={id ? 'Chỉnh sửa quyền' : 'Tạo quyền mới'}
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
          <Grid item xs={12}>
            <InputField
              form={form}
              name="roleName"
              label="Tên quyền *"
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

export default DialogRole;
