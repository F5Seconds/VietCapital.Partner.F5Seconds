import {Divider, Stack} from '@mui/material';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
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
  content: string;
  onClose: () => void;
  onAgree: () => void;
}
const DialogConfirm: FC<Props> = ({open, title, content, onClose, onAgree}) => {
  return (
    <Dialog open={open} TransitionComponent={Transition} keepMounted onClose={onClose}>
      <DialogTitle sx={{fontSize: 16}}>{title}</DialogTitle>
      <Divider />
      <DialogContent>
        <DialogContentText>{content}</DialogContentText>
      </DialogContent>
      <Divider />
      <DialogActions>
        <Stack direction="row" flex={1} justifyContent="center">
          <Button variant="outlined" color="error" onClick={onClose} sx={{minWidth: 150}}>
            Từ chối
          </Button>
          <div style={{width: 24}} />
          <Button onClick={onAgree} variant="contained" color="success" sx={{minWidth: 150}}>
            Đồng ý
          </Button>
        </Stack>
      </DialogActions>
    </Dialog>
  );
};

export default DialogConfirm;
