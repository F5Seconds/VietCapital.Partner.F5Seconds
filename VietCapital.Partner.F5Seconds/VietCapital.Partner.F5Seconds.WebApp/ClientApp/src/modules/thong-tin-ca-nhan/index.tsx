import {Box, Button, Card, CardContent, Container, Grid} from '@mui/material';
import {useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {InputField} from '../../components/hook-form';
import Header from '../../layouts/Header';
import {useAppSelector} from '../../redux/hooks';
import {selectAuth} from '../../redux/slice/auth';
import {accountService} from '../../services';
import DialogResetPassword from './dialog-doi-mat-khau';

type Props = {};

const ThongTinCaNhanPage = (props: Props) => {
  const [openDialog, setOpenDialog] = useState(false);
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
    formState: {isSubmitting},
    reset,
  } = form;
  const auth = useAppSelector(selectAuth);
  useEffect(() => {
    reset({
      lastName: auth?.lastName,
      firstName: auth?.firstName,
      email: auth?.email,
      username: auth?.userName,
    });
  }, [auth]);

  const onSubmit = async (data: {
    lastName: string;
    firstName: string;
    email: string;
    username: string;
  }) => {
    auth?.id && (await accountService.updateUser(auth.id, data));
  };

  const handleSubmitUser = (data: {password: string; confirmPassword: string}) => {
    auth?.email &&
      auth?.jwToken &&
      accountService.resetPassword({...data, email: auth?.email, token: auth?.jwToken});
  };
  return (
    <>
      <Header title="Thông tin cá nhân" />
      <Container sx={{mt: 4}} maxWidth="md">
        <Card variant="outlined">
          <CardContent>
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6} xl={6}>
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
              <Grid item xs={12} sm={6} xl={6}>
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
              <Grid item xs={12} sm={6} xl={6}>
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
              <Grid item xs={12} sm={6} xl={6}>
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
              <Grid item xs={12}>
                <Box display="flex" justifyContent="flex-end">
                  <Button variant="outlined" onClick={() => setOpenDialog(true)}>
                    Đổi mật khẩu
                  </Button>
                  <Box sx={{m: 1}} />
                  <Button variant="contained" onClick={handleSubmit(onSubmit)}>
                    Cập nhật
                  </Button>
                </Box>
              </Grid>
            </Grid>
          </CardContent>
        </Card>

        {openDialog && (
          <DialogResetPassword
            open={openDialog}
            onSubmit={handleSubmitUser}
            onClose={() => setOpenDialog(false)}
          />
        )}
      </Container>
    </>
  );
};

export default ThongTinCaNhanPage;
