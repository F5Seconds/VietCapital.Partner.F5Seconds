import {Outlet, RouteObject} from 'react-router-dom';
import {NotFound} from '../components';
import MainLayout from '../layouts';
import LoginPage from '../modules/auth/login';
import DanhSachUser from '../modules/quan-ly-user/danh-sach-user';
import PhanQuyenUser from '../modules/quan-ly-user/phan-quyen-user';
import DanhSachSanPhamPage from '../modules/san-pham/danh-sach';
import ChiTietSanPhamPage from '../modules/san-pham/chi-tiet';
import DanhSachDanhMucPage from '../modules/danh-muc/danh-sach';
import ChiTietDanhMucPage from '../modules/danh-muc/chi-tiet';

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
      {
        path: 'san-pham',
        element: <DanhSachSanPhamPage />,
        children: [
          {
            path: 'them-san-pham',
            element: <ChiTietSanPhamPage />,
          },
          {
            path: 'sua-san-pham',
            element: <ChiTietSanPhamPage />,
          },
        ],
      },
      {
        path: 'danh-muc',
        element: <DanhSachDanhMucPage />,
        children: [
          {
            path: 'them-danh-muc',
            element: <ChiTietDanhMucPage />,
          },
          {
            path: 'sua-danh-muc',
            element: <ChiTietDanhMucPage />,
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
