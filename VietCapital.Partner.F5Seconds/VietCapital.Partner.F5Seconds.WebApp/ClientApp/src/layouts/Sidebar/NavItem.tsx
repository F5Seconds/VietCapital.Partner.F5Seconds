import {Button, Collapse, ListItem, Stack} from '@mui/material';
import {ArrowDown2, ArrowUp2} from 'iconsax-react';
import React, {FC, useState} from 'react';
import {matchPath, NavLink as RouterLink, useLocation, useNavigate} from 'react-router-dom';
import {colors} from '../../theme';

interface Props {
  item: {
    href: string;
    icon: any;
    title: string;
    children?: Array<any>;
  };
  onClick?: () => void;
  rest?: any;
  listQuyen?: string[];
}
const NavItem: FC<Props> = ({item, onClick, listQuyen, ...rest}) => {
  const {href, icon: Icon, title, children = []} = item;
  const location = useLocation();
  const navigate = useNavigate();
  const [childrenActive, setChildrenActive] = useState(true);
  // href === '/so-hoa/danh-sach-phieu-yeu-cau' ? true : false
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
    if (onClick) {
      onClick();
    } else if (children.length !== 0) {
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
          py: 0.5,
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
              backgroundColor: colors.primaryDark,
            },
            width: '100%',
            ...(active && {
              backgroundColor: colors.primaryDark,
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
          {children.length !== 0 &&
            (childrenActive ? (
              <ArrowUp2 size="20" color="#fff" />
            ) : (
              <ArrowDown2 size="20" color="#fff" />
            ))}
        </Button>
      </ListItem>
      {children.length !== 0 && (
        <Collapse in={childrenActive}>
          {children.map(item => {
            const newListQuyen = Array.isArray(listQuyen)
              ? listQuyen
                  ?.filter((item: string) => item?.split(';')[1] === 'seen')
                  ?.map((item: string) => `${item?.split(';')[0]}`)
              : [];
            if (newListQuyen?.indexOf(item?.href) > -1) {
              return (
                <ListItem
                  key={item.href}
                  disableGutters
                  sx={{
                    display: 'flex',
                    py: 0.5,
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
                        backgroundColor: colors.primaryDark,
                      },
                      width: '100%',
                      // ...(active && {
                      //   backgroundColor: '#08488C',
                      // }),
                      backgroundColor: location.pathname.includes(item.href?.split('/')[2])
                        ? colors.primaryDark
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
              );
            }
            return null;
          })}
        </Collapse>
      )}
    </>
  );
};

export default NavItem;
