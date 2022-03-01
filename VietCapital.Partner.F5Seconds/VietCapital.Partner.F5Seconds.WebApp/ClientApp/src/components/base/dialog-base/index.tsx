import {LoadingButton} from '@mui/lab';
import {Dialog, DialogActions, DialogContent, DialogTitle, Divider, Stack} from '@mui/material';
import Slide from '@mui/material/Slide';
import {TransitionProps} from '@mui/material/transitions';
import React, {FC} from 'react';

const Transition = React.forwardRef(function Transition(
  props: TransitionProps & {
    children: React.ReactElement<any, any>;
  },
  ref: React.Ref<unknown>
) {
  return <Slide direction="up" ref={ref} {...props} />;
});

interface Props {
  open: boolean;
  title: string;
  children: React.ReactNode;
  onClose: () => void;
  onSubmit: () => void;
  isSubmitting?: boolean;
  textNegative?: string;
  textPositive?: string;
}
const DialogBase: FC<Props> = ({
  open,
  title,
  children,
  onClose,
  onSubmit,
  isSubmitting = false,
  textNegative = 'Thoát',
  textPositive = 'Xác nhận',
  ...rest
}) => {
  return (
    <Dialog
      open={open}
      TransitionComponent={Transition}
      keepMounted
      onClose={onClose}
      fullWidth
      maxWidth="md"
      {...rest}
      onClick={e => {
        e.stopPropagation();
      }}
    >
      <DialogTitle sx={{fontSize: 16}}>{title}</DialogTitle>
      <Divider />
      <DialogContent>{children}</DialogContent>
      <Divider />
      <DialogActions>
        <Stack flex={1} direction="row" justifyContent="flex-end">
          <LoadingButton
            disabled={isSubmitting}
            variant="outlined"
            color="inherit"
            onClick={onClose}
            sx={{minWidth: 150}}
          >
            {textNegative}
          </LoadingButton>
          <div style={{width: 24}} />
          <LoadingButton
            loading={isSubmitting}
            variant="contained"
            color="success"
            onClick={onSubmit}
            sx={{minWidth: 150}}
          >
            {textPositive}
          </LoadingButton>
        </Stack>
      </DialogActions>
    </Dialog>
  );
};

export default DialogBase;
