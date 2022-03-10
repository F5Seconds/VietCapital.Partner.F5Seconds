import {FC} from 'react';
import {Outlet, RouteObject, Navigate} from 'react-router-dom';
import {NotFound} from '../components';
import MainLayout from '../layouts';
import LoginPage from '../modules/auth/login';
import DanhSachUser from '../modules/quan-ly-user/danh-sach-user';
import PhanQuyenUser from '../modules/quan-ly-user/phan-quyen-user';
import {useAppSelector} from '../redux/hooks';
import {selectJWT} from '../redux/slice/auth';

const Auth: FC<{children: React.ReactElement}> = ({children}) => {
  const jwt = useAppSelector(selectJWT);

  if (!jwt) {
    return <Navigate to="/login" />;
  }
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
      {path: '*', element: <NotFound />},
    ],
  },
  {
    path: '/login',
    element: <LoginPage />,
  },
];
