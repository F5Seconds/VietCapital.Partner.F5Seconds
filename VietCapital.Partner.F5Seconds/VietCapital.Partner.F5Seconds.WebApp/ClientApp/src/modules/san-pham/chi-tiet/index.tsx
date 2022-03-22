/* eslint-disable react-hooks/exhaustive-deps */
import {Button, Card, Grid, List, ListItem, ListItemText, Stack, Typography} from '@mui/material';
import React, {useCallback, useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {useNavigate, useParams} from 'react-router-dom';
import {CardBase} from '../../../components/base';
import LoadingOverlay from '../../../components/base/loading-overlay';
import {
  AutocompleteAsyncField,
  CheckboxField,
  ImagePickerField,
  InputField,
  TextAreaField,
} from '../../../components/hook-form';
import useCheckQuyen from '../../../hooks/useCheckQuyen';
import Page from '../../../layouts/Page';
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
  categories: [],
  image: '',
  thumbnail: '',
  content: '',
  term: '',
  status: false,
};
const ChiTietSanPhamPage = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [diaDiems, setDiaDiems] = useState<any[]>([]);
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

  const onSubmit = async (data: Partial<Product & {categoryProducts?: any}>) => {
    // console.log('====================================');
    console.log(data);
    // console.log('====================================');
    if (id) {
      await productService.update(id, {
        id,
        ...data,
        categoryProducts:
          data.categories?.map((item: any) => ({categoryId: item.value, productId: id})) || [],
      });
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
    getCategories('');
  }, []);
  useEffect(() => {
    const getDetail = async () => {
      const res: any = await productService.getOne(id);
      if (res) {
        Object.keys(defaultValues).forEach((item: any) => {
          if (item === 'categories') {
            res.categories &&
              setValue(
                'categories',
                res.categories?.map((i: any) => ({label: i.name, value: i.id}))
              );
          } else if (typeof res[item] === 'number') {
            setValue(item, res[item] + '');
          } else if (item === 'status') {
            console.log('status', res[item]);
            setValue(item, res[item]);
          } else {
            setValue(item, res[item]);
          }
        });
      }
    };
    id && getDetail();
  }, [id]);
  const [checkQuyen] = useCheckQuyen();

  return (
    <Page title={id ? 'Cập nhật sản phẩm' : 'Thêm sản phẩm'}>
      <Grid sx={{pointerEvents: 'none'}} container spacing={2}>
        <Grid item xs={12} md={12} lg={12}>
          <CardBase
            actions={
              checkQuyen('edit') ? (
                <Stack direction="row" justifyContent="flex-end" margin={2}>
                  <Button variant="contained" color="primary" onClick={handleSubmit(onSubmit)}>
                    {id ? 'Cập nhật' : 'Thêm sản phẩm'}
                  </Button>
                </Stack>
              ) : (
                false
              )
            }
          >
            <Grid container sx={{padding: 2}} spacing={2}>
              <Grid item xs={12} md={6} lg={4}>
                <InputField form={form} name="productCode" label="Mã sản phẩm" disabled />
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <InputField form={form} name="type" label="Loại" disabled />
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <InputField form={form} name="price" label="Giá" disabled />
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <InputField form={form} name="brandName" label="Tên thương hiệu" disabled />
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <InputField
                  form={form}
                  name="brandLogo"
                  label="Đường dẫn logo thương hiệu"
                  disabled
                />
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <InputField form={form} name="partner" label="Nhà phân phối" disabled />
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
                  items={categories?.map(item => ({label: item?.name, value: item?.id, ...item}))}
                  onSubmit={value => getCategories(value)}
                  form={form}
                  name="categories"
                  label="Danh mục"
                />
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                {/* <InputField form={form} name="image" label="Đường dẫn ảnh sản phẩm" /> */}
                <ImagePickerField form={form} name="image" label="Hình ảnh" />
              </Grid>

              <Grid item xs={12} md={6} lg={4}>
                <ImagePickerField
                  form={form}
                  name="thumbnail"
                  label="Đường dẫn ảnh sản phẩm thu nhỏ"
                />
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <CheckboxField form={form} name="status" label="Trạng thái" />
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
              <Grid item xs={12} md={12} lg={12}>
                <Typography component="div" color="text.secondary">
                  Cửa hàng áp dụng
                </Typography>
                <Card>
                  <List>
                    {diaDiems.length === 0 && (
                      <ListItem>
                        <ListItemText
                          sx={{textAlign: 'center'}}
                          primary="Chưa có cửa hàng áp dụng"
                        />
                      </ListItem>
                    )}
                    {diaDiems.map(item => (
                      <ListItem>
                        <ListItemText primary={item} />
                      </ListItem>
                    ))}
                  </List>
                </Card>
              </Grid>
            </Grid>
          </CardBase>
        </Grid>

        {/* <Grid item xs={12} md={12} lg={4}>
            <CardBase title="Địa điểm áp dụng" headerShow></CardBase>
          </Grid> */}
      </Grid>

      <LoadingOverlay open={isSubmitting} />
    </Page>
  );
};

export default ChiTietSanPhamPage;
