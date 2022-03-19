import {Box} from '@mui/system';
import React from 'react';
import {Outlet} from 'react-router-dom';
import Sidebar from './Sidebar';
const MainLayout = () => {
  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateRows: 'auto 1fr',
        gridTemplateColumns: '256px 1fr',
        gridTemplateAreas: `"sidebar main" "sidebar main"`,
        minHeight: '100vh',
        minWidth: '100vw',
        backgroundColor: 'transparent',
      }}
    >
      <Box sx={{gridArea: 'sidebar'}}>
        <Sidebar />
      </Box>
      <Box sx={{gridArea: 'main', padding: 0, backgroundColor: '#f3f6f9'}}>
        <Outlet />
      </Box>
    </Box>
  );
};

export default MainLayout;
