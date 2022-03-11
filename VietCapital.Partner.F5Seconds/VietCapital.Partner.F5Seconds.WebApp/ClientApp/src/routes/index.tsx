import {FC} from 'react';
import {Navigate, Outlet, RouteObject} from 'react-router-dom';
import {NotFound} from '../components';
import MainLayout from '../layouts';
import LoginPage from '../modules/auth/login';
import ChiTietDanhMucPage from '../modules/danh-muc/chi-tiet';
import DanhSachDanhMucPage from '../modules/danh-muc/danh-sach';
import DanhSachDonHangPage from '../modules/don-hang/danh-sach';
import DanhSachUser from '../modules/quan-ly-user/danh-sach-user';
import PhanQuyenUser from '../modules/quan-ly-user/phan-quyen-user';
import ChiTietSanPhamPage from '../modules/san-pham/chi-tiet';
import DanhSachSanPhamPage from '../modules/san-pham/danh-sach';
import {useAppSelector} from '../redux/hooks';
import {selectJWT} from '../redux/slice/auth';

const Auth: FC<{children: React.ReactElement}> = ({children}) => {
  const jwt = useAppSelector(selectJWT);

  // if (!jwt) {
  //   return <Navigate to="/login" />;
  // }
  return children;
};
export const routes: RouteObject[] = [
  {
    path: '/',
    element: (
      <Auth>
        <MainLayout />
      </Auth>
    ),
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
        element: <Outlet />,
        children: [
          {
            index: true,
            element: <DanhSachSanPhamPage />,
          },
          {
            path: 'them-san-pham',
            element: <ChiTietSanPhamPage />,
          },
          {
            path: 'sua-san-pham/:id',
            element: <ChiTietSanPhamPage />,
          },
        ],
      },
      {
        path: 'danh-muc',
        element: <Outlet />,
        children: [
          {
            index: true,
            element: <DanhSachDanhMucPage />,
          },
          {
            path: 'them-danh-muc',
            element: <ChiTietDanhMucPage />,
          },
          {
            path: 'sua-danh-muc/:id',
            element: <ChiTietDanhMucPage />,
          },
        ],
      },
      {
        path: 'don-hang',
        element: <DanhSachDonHangPage />,
        children: [
          {
            path: 'chi-tiet',
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
