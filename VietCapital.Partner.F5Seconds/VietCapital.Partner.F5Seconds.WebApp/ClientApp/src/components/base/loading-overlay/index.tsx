import React, {FC} from 'react';
import {Backdrop, CircularProgress} from '@mui/material';

const LoadingOverlay: FC<{open: boolean}> = ({open}) => {
  return (
    <Backdrop
      sx={{
        color: '#fff',
        zIndex: theme => theme.zIndex.drawer + 10,
        width: '100%',
        height: '100%',
      }}
      open={open}
    >
      <CircularProgress color="inherit" />
    </Backdrop>
  );
};

export default LoadingOverlay;
