import {Card, CardActions, CardContent, Divider, Typography} from '@mui/material';
import React, {FC} from 'react';

interface Props {
  headerShow?: boolean;
  title?: string;
  maxHeight?: number;
  children: React.ReactNode | React.ReactChild;
  actions: React.ReactNode | React.ReactChild;
}
const CardBase: FC<Props> = props => {
  const {children, actions, headerShow = false, maxHeight, title, ...rest} = props;
  return (
    <Card {...rest} elevation={3} sx={{maxHeight, borderRadius: 1}} variant="outlined">
      {headerShow && (
        <>
          <Typography variant="h5" margin={2}>
            {title}
          </Typography>
          <Divider />
        </>
      )}
      <CardContent sx={{overflowY: 'auto', maxHeight: maxHeight ? maxHeight - 152 : null}}>
        {children}
      </CardContent>
      {actions && <CardActions>{actions}</CardActions>}
    </Card>
  );
};
export default CardBase;
