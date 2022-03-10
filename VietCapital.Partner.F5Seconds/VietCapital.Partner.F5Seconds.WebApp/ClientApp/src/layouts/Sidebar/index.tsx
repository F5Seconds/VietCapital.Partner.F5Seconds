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
import {KeyboardOpen, TaskSquare} from 'iconsax-react';
import {FC, useEffect, useState} from 'react';
import {useDispatch} from 'react-redux';
import {useLocation, useNavigate} from 'react-router-dom';
import {useAppSelector} from '../../redux/hooks';
// import {authActions} from 'src/redux/slice/authSlice';
// import {nhanSuService} from 'src/services';
// import {avatar} from 'src/utils';
import NavItem from './NavItem';

const items = [
  {
    href: '/quan-ly-user',
    icon: TaskSquare,
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
    href: '/san-pham',
    icon: TaskSquare,
    title: 'Danh sách sản phẩm',
  },
  {
    href: '/danh-muc',
    icon: TaskSquare,
    title: 'Danh sách danh mục',
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
  const dispatch = useDispatch();
  const [user, setUser] = useState();
  useEffect(() => {
    if (openMobile && onMobileClose) {
      onMobileClose();
    }
  }, [location.pathname]);

  const content = (
    <Box className="sidebar" sx={{p: 2, overflow: 'auto'}}>
      <List>
        {items.map(item => (
          <NavItem key={item.title} item={item} />
        ))}
      </List>
    </Box>
  );

  // useEffect(() => {
  //   const isLogin = sessionStorage.getItem('user');
  //   const getUser = async () => {
  //     const res = await nhanSuService.getByEmail(email);
  //     if (res) {
  //       console.log(res);
  //       setUser(res);
  //       dispatch(authActions.setUser(res));
  //       sessionStorage.setItem('user', JSON.stringify(res));
  //     }
  //   };
  //   if (!!isLogin) {
  //     setUser(JSON.parse(isLogin));
  //     dispatch(authActions.setUser(JSON.parse(isLogin)));
  //   } else {
  //     email && getUser();
  //   }
  // }, [jwt]);
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
              backgroundColor: '#0c56a5',
            },
          }}
        >
          <Stack display="flex" alignItems="center" style={{padding: 20}}>
            {user ? (
              <Avatar
                onClick={() => navigate('/thong-tin-ca-nhan')}
                sx={{bgcolor: 'red', cursor: 'pointer'}}
              >
                {'avatar.generateName(user?.hoTen)'}
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
            {user ? (
              <Typography variant="h5" component="h5" color="#fff" marginTop={1} textAlign="center">
                {'user?.hoTen'}
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
            {user ? (
              <Typography
                variant="body2"
                component="p"
                color="#fff"
                marginTop={1}
                textAlign="center"
              >
                {'user?.chucVu'}
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
            )}
          </Stack>
          <Divider sx={{background: '#053E7A'}} />
          {content}
        </Drawer>
      </Hidden>
    </>
  );
};

export default Sidebar;
