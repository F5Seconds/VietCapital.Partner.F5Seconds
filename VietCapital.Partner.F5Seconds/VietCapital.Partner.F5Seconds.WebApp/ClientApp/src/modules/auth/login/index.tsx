import React from 'react';
import {Card, Typography, Button, CardActions, CardContent, Grid} from '@mui/material';
import {InputField} from '../../../components/hook-form';
import {useForm} from 'react-hook-form';

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

  const onSubmit = (data: defaultValues) => {
    console.log(data);
  };
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
