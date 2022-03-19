/* eslint-disable react-hooks/exhaustive-deps */
import {Button, Grid, Stack} from '@mui/material';
import {getDownloadURL, ref, uploadBytes} from 'firebase/storage';
import {useSnackbar} from 'notistack';
import React, {useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {useNavigate, useParams} from 'react-router-dom';
import {CardBase} from '../../../components/base';
import DialogMediaUpload from '../../../components/base/dialog-media-upload';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {CheckboxField, FilePickerField, InputField} from '../../../components/hook-form';
import {storage} from '../../../firebase/config';
import Page from '../../../layouts/Page';
import {Category} from '../../../models';
import {categoryService} from '../../../services';

const DanhMucSanPhamPage = () => {
  const {id = ''} = useParams();
  const navigate = useNavigate();
  const {enqueueSnackbar} = useSnackbar();
  const [isUploading, setIsUploading] = useState(false);
  const form = useForm<{name: string; image: any; status: boolean}>({
    defaultValues: {
      name: '',
      image: new Blob(),
      status: true,
    },
  });
  const {
    setValue,
    getValues,
    handleSubmit,
    formState: {isSubmitting},
  } = form;
  const [urlUpload, setUrlUpload] = useState('');
  const onSubmit = async (data: Partial<Category>) => {
    if (id) {
      await categoryService.update(id, {id, ...data, image: urlUpload});
    } else {
      const res = await categoryService.create(data);
      if (res) {
        navigate(-1);
      }
    }
  };
  const handleUpload = async () => {
    console.log(getValues('image'));
    try {
      setIsUploading(true);
      const storageRef = ref(storage, `images/categories/category_${Date.now()}.png`);
      const resUpload = await uploadBytes(storageRef, getValues('image'));
      console.log('Uploaded a blob or file!', resUpload);
      const url = await getDownloadURL(storageRef);
      setUrlUpload(url);
      alert('upload thành công');
      enqueueSnackbar('Upload thành công', {variant: 'success'});
    } catch (error) {
      alert('đã xảy ra lỗi');
      console.log(error);
      enqueueSnackbar('Đã xảy ra lỗi', {variant: 'error'});
    } finally {
      setIsUploading(false);
    }
  };

  useEffect(() => {
    const getDetail = async () => {
      const res = await categoryService.getOne(id);
      if (res) {
        setValue('name', res.name);
        setValue('image', res.image);
        setValue('status', res.status);
        setUrlUpload(res.image);
      }
    };
    id && getDetail();
  }, [id]);
  return (
    <Page title={id ? 'Cập nhật' : 'Thêm danh mục'}>
      <CardBase
        actions={
          <Stack direction="row" justifyContent="flex-end" margin={2}>
            <Button variant="contained" color="primary" onClick={handleUpload}>
              Upload
            </Button>
            <DialogMediaUpload open title="Upload" />
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
            <FilePickerField form={form} name="image" label="Hình ảnh" />
          </Grid>
          <Grid item xs={12} md={6} lg={4}>
            <CheckboxField form={form} name="status" label="Trạng thái" />
          </Grid>
        </Grid>
      </CardBase>
      <LoadingOverlay open={isUploading} />
      <LoadingOverlay open={isSubmitting} />
    </Page>
  );
};

export default DanhMucSanPhamPage;
