import {LoadingButton} from '@mui/lab';
import {
  Button,
  ButtonBase,
  Dialog,
  DialogActions,
  DialogContent,
  DialogProps,
  DialogTitle,
  Divider,
  Grid,
  Stack,
  CircularProgress,
  Checkbox,
} from '@mui/material';
import Slide from '@mui/material/Slide';
import {TransitionProps} from '@mui/material/transitions';
import {getDownloadURL, listAll, ref, uploadBytes} from 'firebase/storage';
import {useSnackbar} from 'notistack';
import React, {FC, useRef, useState} from 'react';
import FileUpload from 'react-material-file-upload';
import {storage} from '../../../firebase/config';

const Transition = React.forwardRef(function Transition(
  props: TransitionProps & {
    children: React.ReactElement<any, any>;
  },
  ref: React.Ref<unknown>
) {
  return <Slide direction="up" ref={ref} {...props} />;
});

interface Props extends DialogProps {
  open: boolean;
  onClose?: () => void;
  textCancel?: string;
  textAccept?: string;
  title: string;
  children?: React.ReactNode[] | React.ReactNode;
  onSubmit?: () => void;
}
const DialogMediaUpload: FC<Props> = ({
  open,
  onClose,
  onSubmit,
  textCancel,
  textAccept,
  title,
  children,
  ...rest
}) => {
  const storageRef = ref(storage, 'images');
  const [listImage, setListImage] = useState<{url: string; name: string; checked: boolean}[]>([]);
  const {enqueueSnackbar} = useSnackbar();
  const [files, setFiles] = useState<File[]>([]);
  const [isUpload, setIsUpload] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const handleSubmit = async () => {
    if (isUpload) {
      setIsSubmitting(true);
      const result = await Promise.all(
        files.map(async item => {
          const storageRef = ref(storage, `images/${item.name}`);
          const resUpload = await uploadBytes(storageRef, item);
          console.log(resUpload);
          return resUpload;
        })
      );
      console.log(result);
      if (result.length > 0) {
        enqueueSnackbar('Upload thành công', {variant: 'success'});
      }
      setIsSubmitting(false);
    } else {
      console.log(listImage);
    }
    console.log(files);
  };
  React.useEffect(() => {
    const getAllImage = async () => {
      try {
        // setIsLoading(true)
        const res = await listAll(storageRef);
        const listImg = await Promise.all(
          res.items.map(async itemRef => {
            const url = await getDownloadURL(itemRef);
            return {url, name: itemRef.name, checked: false};
          })
        );
        setIsLoading(false);
        // const dataImage = await Promise.all(
        //   res.prefixes.map(async folderRef => {
        //     const result = await listAll(folderRef);
        //     const listImg = await Promise.all(
        //       result.items.map(async itemRef => {
        //         const url = await getDownloadURL(itemRef);
        //         return {url, name: itemRef.name};
        //       })
        //     );
        //     return listImg;
        //   })
        // );

        setListImage(listImg);
      } catch (error) {
        console.log('Lỗi get list image');
      }
    };
    !isUpload && getAllImage();
  }, [isUpload]);

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
      <DialogTitle
        sx={{fontSize: 16, display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}
      >
        {title}
        <Button onClick={() => setIsUpload(prev => !prev)}>
          {isUpload ? 'Chọn hình ảnh' : 'Upload hình ảnh'}
        </Button>
      </DialogTitle>
      <Divider />
      <DialogContent>
        {isLoading ? (
          <Stack direction="row" justifyContent="center">
            <CircularProgress />
          </Stack>
        ) : isUpload ? (
          <FileUpload value={files} onChange={setFiles} multiple />
        ) : (
          <Stack direction="row" flexWrap="wrap">
            {listImage.map((item, index) => (
              <div
                style={{
                  position: 'relative',
                  display: 'flex',
                  flexDirection: 'column',
                  justifyContent: 'space-between',
                  backgroundColor: 'rgba(0,0,0,0.2)',
                  marginRight: 16,
                  marginBottom: 16,
                  width: 150,
                  height: 150,
                  padding: 4,
                  cursor: 'pointer',
                }}
              >
                <img
                  style={{
                    width: 100,
                    height: 100,
                    objectFit: 'contain',
                    marginLeft: 'auto',
                    marginRight: 'auto',
                  }}
                  src={item.url}
                  alt={item.name}
                />
                <span
                  style={{
                    whiteSpace: 'nowrap',
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                  }}
                >
                  {item.name}
                </span>
                <Checkbox
                  sx={{position: 'absolute', top: -10, left: -10}}
                  value={item.checked}
                  onChange={(e, checked) => {
                    const newList = [...listImage];
                    newList[index] = {...newList[index], checked: true};
                    setListImage(newList);
                  }}
                />
              </div>
            ))}
          </Stack>
        )}
      </DialogContent>
      <Divider />
      <DialogActions>
        <Stack flex={1} direction="row" justifyContent="flex-end">
          <LoadingButton
            // disabled={isSubmitting}
            variant="outlined"
            onClick={onClose}
            sx={{minWidth: 150}}
          >
            Hủy
          </LoadingButton>
          <div style={{width: 24}} />
          <LoadingButton
            loading={isSubmitting}
            variant="contained"
            sx={{minWidth: 150}}
            onClick={handleSubmit}
          >
            {isUpload ? 'Bắt đầu upload' : 'Hoàn thành'}
          </LoadingButton>
        </Stack>
      </DialogActions>
    </Dialog>
  );
};

export default DialogMediaUpload;
