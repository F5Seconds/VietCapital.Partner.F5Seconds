import {Button, Collapse, ListItem, Stack} from '@mui/material';
import {ArrowDown2, ArrowUp2, IconProps} from 'iconsax-react';
import React, {FC, useState} from 'react';
import {matchPath, NavLink as RouterLink, useLocation, useNavigate} from 'react-router-dom';

interface Props {
  item: {
    href: string;
    icon: any;
    title: string;
    children: Array<any>;
  };
  rest?: any;
}
const NavItem: FC<Props> = ({item, ...rest}) => {
  const {href, icon: Icon, title, children = []} = item;
  const location = useLocation();
  const navigate = useNavigate();
  const [childrenActive, setChildrenActive] = useState(true);
  // href === '/so-hoa/danh-sach-phieu-yeu-cau' ? true : false
  console.log(location.pathname, href);
  const active = href
    ? !!matchPath(
        {
          path: href,
          end: false,
        },
        location.pathname
      )
    : false;
  // const active=
  const handleClickCollapse = () => {
    if (!!children) {
      setChildrenActive(prev => !prev);
    } else {
      navigate(href);
    }
  };
  return (
    <>
      <ListItem
        disableGutters
        sx={{
          display: 'flex',
          py: 0,
        }}
        {...rest}
      >
        <Button
          // component={RouterLink}
          sx={{
            color: '#fff',
            fontWeight: 'medium',
            justifyContent: 'space-between',
            letterSpacing: 0,
            py: 1.25,
            textTransform: 'none',
            '&:hover': {
              backgroundColor: '#08488C',
            },
            width: '100%',
            ...(active && {
              backgroundColor: '#08488C',
            }),
            '& svg': {
              mr: 1,
            },
          }}
          // to={href}
          onClick={handleClickCollapse}
        >
          <Stack direction="row">
            {Icon && <Icon size="20" color="#fff" />}
            <span>{title}</span>
          </Stack>
          {!!children &&
            (childrenActive ? (
              <ArrowUp2 size="20" color="#fff" />
            ) : (
              <ArrowDown2 size="20" color="#fff" />
            ))}
        </Button>
      </ListItem>
      {children && (
        <Collapse in={childrenActive}>
          {children.map(item => (
            <ListItem
              key={item.href}
              disableGutters
              sx={{
                display: 'flex',
                py: 0,
              }}
              {...rest}
            >
              <Button
                component={RouterLink}
                sx={{
                  paddingLeft: 4,
                  color: '#fff',
                  fontWeight: 'medium',
                  justifyContent: 'flex-start',
                  letterSpacing: 0,
                  py: 1.25,
                  textTransform: 'none',
                  '&:hover': {
                    backgroundColor: '#08488C',
                  },
                  width: '100%',
                  // ...(active && {
                  //   backgroundColor: '#08488C',
                  // }),
                  backgroundColor: location.pathname.includes(item.href?.split('/')[2])
                    ? '#08488C'
                    : null,
                  '& svg': {
                    mr: 1,
                  },
                }}
                to={item.href}
              >
                <span>{item.title}</span>
              </Button>
            </ListItem>
          ))}
        </Collapse>
      )}
    </>
  );
};

export default NavItem;
