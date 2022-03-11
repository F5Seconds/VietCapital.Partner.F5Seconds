import {
  CardMedia,
  CircularProgress,
  Dialog,
  DialogContent,
  DialogTitle,
  Divider,
  Grid,
  Slide,
  Stack,
  Typography,
} from '@mui/material';
import {TransitionProps} from '@mui/material/transitions';
import React, {FC, useState} from 'react';
import {useForm} from 'react-hook-form';
import {state, stateColor} from 'utils/state';
import {DialogBase} from '../../../../components/base';
import LoadingOverlay from '../../../../components/base/loading-overlay';
import {InputField} from '../../../../components/hook-form';

interface Props {
  open: boolean;
  row?: any;
  onClose: () => void;
}

const Transition = React.forwardRef(function Transition(
  props: TransitionProps & {
    children: React.ReactElement<any, any>;
  },
  ref: React.Ref<unknown>
) {
  return <Slide direction="up" ref={ref} {...props} />;
});

const DialogDetail: FC<Props> = ({open = false, row, onClose}) => {
  return (
    <Dialog
      open={open}
      TransitionComponent={Transition}
      keepMounted
      onClose={onClose}
      fullWidth
      maxWidth="md"
    >
      <DialogTitle sx={{fontSize: 16}}>Chi tiết</DialogTitle>
      <Divider />
      <DialogContent>
        <Grid container spacing={2}>
          <Grid item xs={12} sm={4} xl={4}>
            <CardMedia
              component="img"
              sx={{width: '100%'}}
              image={row?.product?.image}
              alt="Live from space album cover"
            />
          </Grid>
          <Grid item xs={12} sm={8} xl={8}>
            <Typography component="div" variant="h5">
              {row?.product?.name}
            </Typography>
            <Divider sx={{my: 1}} />

            <Stack direction="row" justifyContent="space-between">
              <Typography variant="subtitle1" color="text.primary" component="div">
                Mã khách hàng:
              </Typography>
              <Typography variant="subtitle1" color="text.secondary" component="div">
                {row?.customerId}
              </Typography>
            </Stack>

            <Stack mt={1} direction="row" justifyContent="space-between">
              <Typography variant="subtitle1" color="text.primary" component="div">
                Mã sản phẩm:
              </Typography>
              <Typography variant="subtitle1" color="text.secondary" component="div">
                {row?.product?.productCode}
              </Typography>
            </Stack>

            <Stack mt={1} direction="row" justifyContent="space-between">
              <Typography variant="subtitle1" color="text.primary" component="div">
                Mã voucher:
              </Typography>
              <Typography variant="subtitle1" color="text.secondary" component="div">
                {row?.voucherCode}
              </Typography>
            </Stack>

            <Stack mt={1} direction="row" justifyContent="space-between">
              <Typography variant="subtitle1" color="text.primary" component="div">
                Điểm:
              </Typography>
              <Typography variant="subtitle1" color="text.secondary" component="div">
                {row?.product?.point}
              </Typography>
            </Stack>

            <Stack mt={1} direction="row" justifyContent="space-between">
              <Typography variant="subtitle1" color="text.primary" component="div">
                Trạng thái:
              </Typography>
              <Typography sx={{color: stateColor(row?.state)}} variant="subtitle1" component="div">
                {state(row?.state)}
              </Typography>
            </Stack>

            <Stack mt={1} direction="row" justifyContent="space-between">
              <Typography variant="subtitle1" color="text.primary" component="div">
                Ngày hết hạn:
              </Typography>
              <Typography variant="subtitle1" color="text.secondary" component="div">
                {new Date(row?.expiryDate).toLocaleDateString('vi')}
              </Typography>
            </Stack>

            <Stack mt={1} direction="column" justifyContent="space-between">
              <Typography variant="subtitle1" color="text.primary" component="div">
                Mô tả:
              </Typography>
              <Typography variant="subtitle1" color="text.secondary" component="div">
                {row?.product?.content}
              </Typography>
            </Stack>
          </Grid>
        </Grid>
      </DialogContent>
    </Dialog>
  );
};

export default DialogDetail;
