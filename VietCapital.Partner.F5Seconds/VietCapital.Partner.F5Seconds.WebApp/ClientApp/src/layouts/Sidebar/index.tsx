import {
  Avatar,
  Box,
  Divider,
  Drawer,
  Hidden,
  List,
  Skeleton,
  Stack,
  Typography,
} from '@mui/material';
import {
  Bag2,
  Chart1,
  Dash,
  Firstline,
  KeyboardOpen,
  Logout,
  TaskSquare,
  UserOctagon,
} from 'iconsax-react';
import {FC, useEffect, useState} from 'react';
import {useLocation, useNavigate} from 'react-router-dom';
import {useAppDispatch, useAppSelector} from '../../redux/hooks';
import {selectJWT, setAuth} from '../../redux/slice/auth';
import {accountService} from '../../services';
import {colors} from '../../theme';
// import {authActions} from 'src/redux/slice/authSlice';
// import {nhanSuService} from 'src/services';
// import {avatar} from 'src/utils';
import NavItem from './NavItem';
import jwt_decode from 'jwt-decode';

export const items = [
  {
    href: '/thong-ke',
    icon: Chart1,
    title: 'Thống kê',
  },
  {
    href: '/quan-ly-user',
    icon: UserOctagon,
    title: 'Quản lý user',
    children: [
      {
        href: '/quan-ly-user/danh-sach-user',
        title: 'Danh sách user',
      },
      {
        href: '/quan-ly-user/phan-quyen-user',
        title: 'Phân quyền user',
      },
    ],
  },
  {
    href: '/danh-muc',
    icon: TaskSquare,
    title: 'Danh sách danh mục',
  },
  {
    href: '/san-pham',
    icon: Firstline,
    title: 'Danh sách sản phẩm',
  },
  {
    href: '/don-hang',
    icon: Bag2,
    title: 'Quản lý đơn hàng',
    children: [
      {
        href: '/don-hang/danh-sach-don-hang',
        title: 'Danh sách đơn hàng',
      },
      {
        href: '/don-hang/doi-soat',
        title: 'Đối soát',
      },
    ],
  },

  // {
  //   href: '/nhac-viec',
  //   icon: Notification,
  //   title: 'Nhắc việc của bạn',
  // },
];

const itemsQL = [
  {
    href: '/quan-ly',
    icon: KeyboardOpen,
    title: 'Quản lý',
    children: [
      {
        href: '/quan-ly/don-vi',
        title: 'Đơn vị',
      },
      {
        href: '/quan-ly/nhom-chuong-trinh',
        title: 'Nhóm chương trình',
      },
      {
        href: '/quan-ly/chuong-trinh',
        title: 'Chương trình',
      },
      {
        href: '/quan-ly/bo-phan',
        title: 'Bộ phận',
      },
      {
        href: '/quan-ly/phan-quyen',
        title: 'Phân quyền',
      },
    ],
  },
];

interface Props {
  onMobileClose?: () => void;
  openMobile?: boolean;
}
const Sidebar: FC<Props> = ({onMobileClose, openMobile}) => {
  const location = useLocation();
  const navigate = useNavigate();
  // const {jwt, email} = useAppSelector(state => state.auth);
  const dispatch = useAppDispatch();

  const jwt = localStorage.getItem('jwt');
  const auth: any = jwt && jwt_decode(jwt);

  useEffect(() => {
    if (openMobile && onMobileClose) {
      onMobileClose();
    }
  }, [location.pathname]);

  useEffect(() => {
    console.log(auth?.quyen);
  }, [auth]);

  const content = (
    <Box className="sidebar" sx={{p: 2, overflow: 'auto'}}>
      <List>
        {items.map(item => {
          const listQuyen = Array.isArray(auth?.quyen)
            ? auth?.quyen
                ?.filter((item: string) => item?.split(';')[1] === 'seen')
                ?.map((item: string) => `/${item.split('/')[1]?.split(';')[0]}`)
            : [];
          console.log({listQuyen});
          if (listQuyen?.indexOf(item.href) > -1) {
            return <NavItem key={item.title} item={item} listQuyen={auth?.quyen} />;
          }
          return null;
        })}
        <NavItem
          onClick={() => {
            dispatch(setAuth({}));
            localStorage.removeItem('jwt');
          }}
          item={{
            href: '/login',
            icon: Logout,
            title: 'Đăng xuất',
          }}
        />
      </List>
    </Box>
  );

  return (
    <>
      <Hidden>
        <Drawer
          anchor="left"
          open
          variant="persistent"
          PaperProps={{
            sx: {
              width: 256,
              background:
                '#133886 url(https://www.vietcapitalbank.com.vn/static/images/mask-logo.png) no-repeat right bottom',
            },
          }}
          sx={{position: 'relative'}}
        >
          <Stack display="flex" alignItems="center" style={{padding: 20}}>
            {auth ? (
              <Avatar
                onClick={() => navigate('/thong-tin-ca-nhan')}
                sx={{bgcolor: 'red', cursor: 'pointer'}}
              >
                {auth?.email[0]?.toUpperCase()}
              </Avatar>
            ) : (
              <Skeleton
                sx={{bgcolor: 'white.700'}}
                variant="circular"
                width={40}
                height={40}
                animation="wave"
              />
            )}
            {auth ? (
              <Typography variant="h5" component="h5" color="#fff" marginTop={1} textAlign="center">
                {auth?.email}
              </Typography>
            ) : (
              <Skeleton
                sx={{bgcolor: 'white.700'}}
                variant="text"
                width={150}
                height={30}
                animation="wave"
              />
            )}
            {/* {auth ? (
              <Typography
                variant="body2"
                component="p"
                color="#fff"
                marginTop={1}
                textAlign="center"
              >
                {auth?.roles}
              </Typography>
            ) : (
              <Skeleton
                sx={{bgcolor: 'white.700'}}
                variant="text"
                width={200}
                height={40}
                color="inherit"
                animation="wave"
              />
            )} */}
          </Stack>
          <Divider sx={{background: colors.primary}} />
          {content}
        </Drawer>
      </Hidden>
    </>
  );
};

export default Sidebar;
