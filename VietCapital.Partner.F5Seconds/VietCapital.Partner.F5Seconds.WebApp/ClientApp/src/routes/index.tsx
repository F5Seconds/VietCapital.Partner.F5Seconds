import type {RouteObject} from 'react-router-dom';
import {NotFound} from '../components';
import MainLayout from '../layouts';
import LoginPage from '../modules/auth/login';

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <MainLayout />,
    children: [
      {index: true, element: <div>home</div>},
      {path: '*', element: <NotFound />},
    ],
  },
  {
    path: '/login',
    element: <LoginPage />,
  },
];
