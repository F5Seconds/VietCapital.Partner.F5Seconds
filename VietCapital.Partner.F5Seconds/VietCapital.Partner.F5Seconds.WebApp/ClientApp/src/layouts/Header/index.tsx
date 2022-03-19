import {AppBar, IconButton, Stack, Toolbar, Typography} from '@mui/material';
import {ArrowLeft2} from 'iconsax-react';

import React, {FC} from 'react';
import {useNavigate} from 'react-router-dom';
import {colors} from '../../theme';

interface Props {
  title: string;
  actions?: any;
}
const Header: FC<Props> = props => {
  const {title, actions} = props;
  const navigate = useNavigate();
  return (
    <AppBar elevation={3} {...props} position="sticky">
      <Toolbar
        variant="dense"
        sx={{
          backgroundColor: '#fff',
          display: 'flex',
          justifyContent: 'space-between',
          minHeight: '64px',
        }}
      >
        <Stack direction="row" alignItems="center">
          <IconButton
            size="small"
            edge="start"
            sx={{
              mr: 2,
              ml: 0,
              borderRadius: 1,
              backgroundColor: colors.primary,
              '&.MuiIconButton-root:hover': {
                backgroundColor: colors.primaryDark,
              },
            }}
            onClick={() => navigate(-1)}
          >
            <ArrowLeft2 size="16" color="#fff" />
          </IconButton>

          <Typography variant="h5">{title}</Typography>
        </Stack>
        {actions}
      </Toolbar>
    </AppBar>
  );
};

export default Header;
