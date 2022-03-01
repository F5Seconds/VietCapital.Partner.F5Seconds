import React from 'react';
import {Card, Typography, Button, CardActions, CardContent, Grid} from '@mui/material';
import {InputField} from '../../../components/hook-form';
import {useForm} from 'react-hook-form';
import {accountApi} from '../../../apis';
import {useSnackbar} from 'notistack';
import {Navigate, useNavigate} from 'react-router';
import {useAppSelector, useAppDispatch} from '../../../redux/hooks';
import {selectJWT, setAuth} from '../../../redux/reducer/auth';

interface defaultValues {
  username: string;
  password: string;
}
const LoginPage = () => {
  const form = useForm({
    defaultValues: {
      username: '',
      password: '',
    },
  });
  const {handleSubmit} = form;

  const {enqueueSnackbar} = useSnackbar();
  const navigate = useNavigate();

  const jwt = useAppSelector(selectJWT);
  const dispatch = useAppDispatch();
  console.log(jwt);

  const onSubmit = (data: defaultValues) => {
    console.log(data);
    accountApi
      .login(data.username, data.password)
      .then(res => {
        if (res.data.succeeded) {
          enqueueSnackbar('Đăng nhập thành công', {variant: 'success'});
          localStorage.setItem('jwt', res.data?.data?.jwToken);
          dispatch(setAuth(res.data?.data));
          navigate('/', {replace: true});
        }
      })
      .catch(error => {
        console.log(error);
        enqueueSnackbar('Đăng nhập thất bại', {variant: 'error'});
      });
  };

  if (jwt) {
    return <Navigate to="/" />;
  }
  return (
    <div
      style={{minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center'}}
    >
      <Card sx={{minWidth: 275, padding: 2}}>
        <Typography variant="h5" marginY={2} textAlign="center">
          Đăng nhập
        </Typography>
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12}>
              <InputField
                form={form}
                name="username"
                label="Tên đăng nhập"
                rules={{
                  required: {
                    value: true,
                    message: 'Vui lòng nhập tên đăng nhập',
                  },
                }}
              />
            </Grid>
            <Grid item xs={12}>
              <InputField
                form={form}
                name="password"
                label="Mật khẩu"
                type="password"
                rules={{
                  required: {
                    value: true,
                    message: 'Vui lòng nhập mật khẩu',
                  },
                }}
              />
            </Grid>
          </Grid>
        </CardContent>
        <CardActions>
          <Button variant="contained" fullWidth onClick={handleSubmit(onSubmit)}>
            Đăng nhập
          </Button>
        </CardActions>
      </Card>
    </div>
  );
};

export default LoginPage;
