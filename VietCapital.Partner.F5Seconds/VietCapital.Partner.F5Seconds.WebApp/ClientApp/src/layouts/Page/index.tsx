import React, {FC, ReactNode} from 'react';
import Header from '../Header';
import {styled} from '@mui/material/styles';
import {Container} from '@mui/material';

interface PageProps {
  children: ReactNode;
  title: string;
}

const APP_BAR_MOBILE = 44;
const APP_BAR_DESKTOP = 44;

const MainStyle = styled('div')(({theme}) => ({
  flexGrow: 1,
  overflow: 'auto',
  minHeight: '100%',
  paddingTop: APP_BAR_MOBILE,
  paddingBottom: theme.spacing(2),
  [theme.breakpoints.up('lg')]: {
    paddingTop: APP_BAR_DESKTOP,
    paddingLeft: theme.spacing(2),
    paddingRight: theme.spacing(2),
  },
}));

const Page: FC<PageProps> = ({title, children}) => {
  return (
    <>
      <Header title={title} />
      <MainStyle>
        <Container maxWidth={'lg'}>{children}</Container>
      </MainStyle>
    </>
  );
};

export default Page;
