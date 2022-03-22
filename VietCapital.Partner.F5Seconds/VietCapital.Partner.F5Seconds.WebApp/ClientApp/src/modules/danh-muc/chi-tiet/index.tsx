/* eslint-disable react-hooks/exhaustive-deps */
import {Button, Grid, Stack} from '@mui/material';
import React, {useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {useNavigate, useParams} from 'react-router-dom';
import {CardBase} from '../../../components/base';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {CheckboxField, ImagePickerField, InputField} from '../../../components/hook-form';
import Page from '../../../layouts/Page';
import {Category} from '../../../models';
import {categoryService} from '../../../services';
import useCheckQuyen from '../../../hooks/useCheckQuyen';

const DanhMucSanPhamPage = () => {
  const {id = ''} = useParams();
  const navigate = useNavigate();

  const form = useForm<{name: string; image: any; status: boolean}>({
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
    // console.log(data);

    if (id) {
      await categoryService.update(id, {id, ...data});
    } else {
      const res = await categoryService.create(data);
      if (res) {
        navigate(-1);
      }
    }
  };
  // const handleUpload = async () => {
  //   console.log(getValues('image'));
  //   try {
  //     setIsUploading(true);
  //     const storageRef = ref(storage, `images/categories/category_${Date.now()}.png`);
  //     const resUpload = await uploadBytes(storageRef, getValues('image'));
  //     console.log('Uploaded a blob or file!', resUpload);
  //     const url = await getDownloadURL(storageRef);
  //     setUrlUpload(url);
  //     alert('upload thành công');
  //     enqueueSnackbar('Upload thành công', {variant: 'success'});
  //   } catch (error) {
  //     alert('đã xảy ra lỗi');
  //     console.log(error);
  //     enqueueSnackbar('Đã xảy ra lỗi', {variant: 'error'});
  //   } finally {
  //     setIsUploading(false);
  //   }
  // };

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

  const [checkQuyen] = useCheckQuyen();
  if (!checkQuyen('create') && !checkQuyen('edit')) {
    navigate('/404');
  }
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
            <ImagePickerField form={form} name="image" label="Hình ảnh" />
            {/* <FilePickerField form={form} name="image" label="Hình ảnh" /> */}
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
