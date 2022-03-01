import {Outlet, RouteObject} from 'react-router-dom';
import {NotFound} from '../components';
import MainLayout from '../layouts';
import LoginPage from '../modules/auth/login';
import DanhSachUser from '../modules/quan-ly-user/danh-sach-user';
import PhanQuyenUser from '../modules/quan-ly-user/phan-quyen-user';

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <MainLayout />,
    children: [
      {index: true, element: <div>home</div>},
      {
        path: 'quan-ly-user',
        element: <Outlet />,
        children: [
          {
            path: 'danh-sach-user',
            element: <DanhSachUser />,
          },
          {
            path: 'phan-quyen-user',
            element: <PhanQuyenUser />,
          },
        ],
      },
      {path: '*', element: <NotFound />},
    ],
  },
  {
    path: '/login',
    element: <LoginPage />,
  },
];
