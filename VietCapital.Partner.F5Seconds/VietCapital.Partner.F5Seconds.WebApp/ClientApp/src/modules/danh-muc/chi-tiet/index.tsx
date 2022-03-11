/* eslint-disable react-hooks/exhaustive-deps */
import {Button, Card, Grid, Stack} from '@mui/material';
import React, {useEffect} from 'react';
import {useForm} from 'react-hook-form';
import {useParams} from 'react-router-dom';
import {CardBase} from '../../../components/base';
import {CheckboxField, InputField} from '../../../components/hook-form';
import Header from '../../../layouts/Header';
import {categoryService} from '../../../services';

const DanhMucSanPhamPage = () => {
  const {id = ''} = useParams();
  const form = useForm({
    defaultValues: {
      name: '',
      image: '',
      status: true,
    },
  });
  const {setValue, handleSubmit} = form;

  const onSubmit = async (data: any) => {
    console.log(data);
  };
  useEffect(() => {
    const getDetail = async () => {
      const res = await categoryService.getDetail(id);
      if (res) {
        setValue('name', res.name);
        setValue('image', res.image);
        setValue('status', res.status);
      }
    };
    id && getDetail();
  }, [id]);
  return (
    <div>
      <Header title="Chi tiết danh mục" />
      <div style={{padding: 16}}>
        <CardBase
          actions={
            <Stack direction="row" justifyContent="flex-end" margin={2}>
              <Button variant="contained" color="primary" onClick={handleSubmit(onSubmit)}>
                {id ? 'Cập nhật' : 'Thêm danh mục'}
              </Button>
            </Stack>
          }
        >
          <Grid container sx={{padding: 2}} spacing={2}>
            <Grid item xs={12} md={6} lg={4}>
              <InputField form={form} name="name" label="Tên sản phẩm" />
            </Grid>
            <Grid item xs={12} md={6} lg={4}>
              <InputField form={form} name="image" label="Hình ảnh" />
            </Grid>
            <Grid item xs={12} md={6} lg={4}>
              <CheckboxField form={form} name="status" label="Trạng thái" />
            </Grid>
          </Grid>
        </CardBase>
      </div>
    </div>
  );
};

export default DanhMucSanPhamPage;
