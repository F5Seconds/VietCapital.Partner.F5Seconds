/* eslint-disable react-hooks/exhaustive-deps */
import {Button, Grid, Stack} from '@mui/material';
import React, {useEffect} from 'react';
import {useForm} from 'react-hook-form';
import {useNavigate, useParams} from 'react-router-dom';
import {CardBase} from '../../../components/base';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {CheckboxField, InputField} from '../../../components/hook-form';
import Page from '../../../layouts/Page';
import {Category} from '../../../models';
import {categoryService} from '../../../services';

const DanhMucSanPhamPage = () => {
  const {id = ''} = useParams();
  const navigate = useNavigate();
  const form = useForm({
    defaultValues: {
      name: '',
      image: '',
      status: true,
    },
  });
  const {
    setValue,
    handleSubmit,
    formState: {isSubmitting},
  } = form;

  const onSubmit = async (data: Partial<Category>) => {
    if (id) {
      await categoryService.update(id, {id, ...data});
    } else {
      const res = await categoryService.create(data);
      if (res) {
        navigate(-1);
      }
    }
  };
  useEffect(() => {
    const getDetail = async () => {
      const res = await categoryService.getOne(id);
      if (res) {
        setValue('name', res.name);
        setValue('image', res.image);
        setValue('status', res.status);
      }
    };
    id && getDetail();
  }, [id]);
  return (
    <Page title={id ? 'Cập nhật' : 'Thêm danh mục'}>
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
            <InputField form={form} name="name" label="Tên danh mục" />
          </Grid>
          <Grid item xs={12} md={6} lg={4}>
            <InputField form={form} name="image" label="Hình ảnh" />
          </Grid>
          <Grid item xs={12} md={6} lg={4}>
            <CheckboxField form={form} name="status" label="Trạng thái" />
          </Grid>
        </Grid>
      </CardBase>

      <LoadingOverlay open={isSubmitting} />
    </Page>
  );
};

export default DanhMucSanPhamPage;
