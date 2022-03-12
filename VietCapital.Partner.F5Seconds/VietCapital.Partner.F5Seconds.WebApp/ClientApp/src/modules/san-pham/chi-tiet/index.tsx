/* eslint-disable react-hooks/exhaustive-deps */
import {Button, Grid, Stack, Typography} from '@mui/material';
import React, {useCallback, useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {useNavigate, useParams} from 'react-router-dom';
import {CardBase} from '../../../components/base';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {AutocompleteAsyncField, InputField, TextAreaField} from '../../../components/hook-form';
import Header from '../../../layouts/Header';
import {Category, Product} from '../../../models';
import {categoryService, productService} from '../../../services';

const defaultValues = {
  productCode: '',
  type: undefined,
  price: undefined,
  brandName: '',
  brandLogo: '',
  partner: '',
  name: '',
  point: undefined,
  categoryProducts: null,
  image: '',
  thumbnail: '',
  term: '',
};
const ChiTietSanPhamPage = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loadingCategories, setLoadingCategories] = useState(false);

  const {id = ''} = useParams<string>();
  const navigate = useNavigate();
  const form = useForm({
    defaultValues,
  });
  const {
    setValue,
    handleSubmit,
    formState: {isSubmitting},
  } = form;

  const onSubmit = async (data: Partial<Product>) => {
    if (id) {
      await productService.update(id, {id, ...data});
    } else {
      const res = await productService.create(data);
      if (res) {
        navigate(-1);
      }
    }
  };

  const getCategories = useCallback(async (value: any) => {
    setLoadingCategories(true);
    const res = await categoryService.getAll({search: value, pageNumber: 1, pageSize: 100});

    if (res) {
      setCategories(res.data);
    }
    setLoadingCategories(false);
  }, []);

  useEffect(() => {
    const getDetail = async () => {
      const res: any = await productService.getOne(id);
      if (res) {
        Object.keys(defaultValues).forEach((item: any) => setValue(item, res[item]));
      }
    };
    id && getDetail();
  }, [id]);
  return (
    <div>
      <Header title="Chi tiết sản phẩm" />
      <div style={{padding: 16}}>
        <Grid container spacing={2}>
          <Grid item xs={12} md={12} lg={12}>
            <CardBase
              actions={
                <Stack direction="row" justifyContent="flex-end" margin={2}>
                  <Button variant="contained" color="primary" onClick={handleSubmit(onSubmit)}>
                    {id ? 'Cập nhật' : 'Thêm sản phẩm'}
                  </Button>
                </Stack>
              }
            >
              <Grid container sx={{padding: 2}} spacing={2}>
                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="productCode" label="Mã sản phẩm" />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="type" label="Loại" />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="price" label="Giá" />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="brandName" label="Tên thương hiệu" />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="brandLogo" label="Đường dẫn logo thương hiệu" />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="partner" label="Nhà phân phối" />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="name" label="Tên sản phẩm" />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="point" label="Điểm" />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                  <AutocompleteAsyncField
                    multiple
                    loading={loadingCategories}
                    items={categories.map(item => ({label: item?.name, value: item?.id}))}
                    onSubmit={value => getCategories(value)}
                    form={form}
                    name="categoryProducts"
                    label="Danh mục"
                  />
                </Grid>
                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="image" label="Đường dẫn ảnh sản phẩm" />
                </Grid>

                <Grid item xs={12} md={6} lg={4}>
                  <InputField form={form} name="thumbnail" label="Đường dẫn ảnh sản phẩm thu nhỏ" />
                </Grid>
                <Grid item xs={12} md={12} lg={12}>
                  <Typography component="div" color="text.secondary">
                    Mô tả
                  </Typography>
                  <TextAreaField form={form} name="content" label="" />
                </Grid>
                <Grid item xs={12} md={12} lg={12}>
                  <Typography component="div" color="text.secondary">
                    Hướng dẫn
                  </Typography>
                  <TextAreaField form={form} name="term" label="" />
                </Grid>
              </Grid>
            </CardBase>
          </Grid>

          {/* <Grid item xs={12} md={12} lg={4}>
            <CardBase title="Địa điểm áp dụng" headerShow></CardBase>
          </Grid> */}
        </Grid>
      </div>
      <LoadingOverlay open={isSubmitting} />
    </div>
  );
};

export default ChiTietSanPhamPage;
