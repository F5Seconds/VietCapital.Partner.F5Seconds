import {
  Card,
  CircularProgress,
  Grid,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  ListSubheader,
  Stack,
  Switch,
} from '@mui/material';
import {AddSquare, Edit, Eye, Trash} from 'iconsax-react';
import {FC, useEffect, useState} from 'react';
import {useForm} from 'react-hook-form';
import {DialogBase} from '../../../../components/base';
import LoadingOverlay from '../../../../components/base/loading-overlay';
import {AutocompleteAsyncField} from '../../../../components/hook-form';
import {Role} from '../../../../models/role';
import {accountService} from '../../../../services';

interface Props {
  open: boolean;
  id?: number | string | null;
  onClose: () => void;
}

const DialogGanQuyen: FC<Props> = ({open = false, id, onClose}) => {
  const form = useForm({
    defaultValues: {
      role: null,
      user: [],
    },
  });

  const {
    watch,
    handleSubmit,
    setValue,
    formState: {isSubmitting},
  } = form;
  const [isLoading, setIsLoading] = useState(false);
  const [isLoadingRole, setIsLoadingRole] = useState(true);
  const [listRole, setListRole] = useState<Role[]>([]);
  const [listUsers, setListUsers] = useState([]);
  const [checkedList, setCheckedList] = useState<string[]>([]);

  useEffect(() => {
    const getListRole = async () => {
      const res = await accountService.getAllRole();
      if (res) {
        setListRole(res);
      }
      setIsLoadingRole(false);
    };

    const getUsers = async () => {
      const res = await accountService.getAllUser();
      if (res) {
        setListUsers(res?.listUser);
      }
    };

    getListRole();
    getUsers();
  }, []);

  const user: any = watch('user');
  // console.log({user});

  useEffect(() => {
    const getClaim = () => {
      accountService.getAllClaimsByUser({userName: user?.value}).then(res => {
        const list = res?.filter(item => item.type === 'quyen')?.map(item => item.value);
        list && setCheckedList(list);
      });
    };

    if (user?.value) {
      getClaim();
    }
  }, [user]);

  return (
    <DialogBase
      open={open}
      title="Gán quyền"
      onClose={onClose}
      textPositive="Gán quyền"
      hasSubmitButton={false}
      onSubmit={() => {}}
    >
      {isLoading ? (
        <Stack direction="row" justifyContent="center">
          <CircularProgress size={24} />
        </Stack>
      ) : (
        <Grid container spacing={2}>
          {/* <Grid item xs={12}>
            <AutocompleteAsyncField
              form={form}
              name="role"
              label="Quyền *"
              items={listRole?.map(item => ({...item, label: item.name, value: item.name}))}
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
              loading={isLoadingRole}
            />
          </Grid> */}
          <Grid item xs={12}>
            <AutocompleteAsyncField
              form={form}
              name="user"
              label="Nhân viên *"
              items={listUsers?.map((item: any) => ({
                ...item,
                label: item.username,
                value: item.username,
              }))}
              loading={isLoadingRole}
            />
          </Grid>

          {user?.value && (
            <>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    userName={user.username}
                    title="Danh sách user"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/quan-ly-user/danh-sach-user"
                    listQuyen={['seen', 'create', 'edit', 'delete']}
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    userName={user.username}
                    title="Phân quyền user"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/quan-ly-user/phan-quyen-user"
                    listQuyen={['seen', 'create', 'edit', 'delete']}
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    userName={user.username}
                    title="Danh mục"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/danh-muc"
                    listQuyen={['seen', 'create', 'edit', 'delete']}
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    userName={user.username}
                    title="Sản phẩm"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/san-pham"
                    listQuyen={['seen', 'edit']}
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    userName={user.username}
                    title="Danh sách đơn hàng"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/don-hang/danh-sach-don-hang"
                    listQuyen={['seen']}
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    userName={user.username}
                    title="Đối soát"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/don-hang/doi-soat"
                    listQuyen={['seen']}
                  />
                </Card>
              </Grid>
            </>
          )}
        </Grid>
      )}
      <LoadingOverlay open={isSubmitting} />
    </DialogBase>
  );
};

interface ListThaoTacProps {
  userName: string;
  title: string;
  checkedList: string[];
  onChange: (value: string[]) => void;
  id: string;
  listQuyen: ('seen' | 'create' | 'edit' | 'delete')[];
}

export const ListThaoTac: FC<ListThaoTacProps> = ({
  userName,
  checkedList,
  onChange,
  title,
  listQuyen = [],
  id,
}) => {
  const [loading, setLoading] = useState<{[x: string]: boolean}>({
    [`${id};seen`]: false,
    [`${id};edit`]: false,
  });

  const handleToggle = (value: string) => async () => {
    setLoading(prev => ({...prev, [value]: true}));

    const currentIndex = checkedList.indexOf(value);
    const newChecked = [...checkedList];

    if (currentIndex === -1) {
      const res = await accountService.addClaimToUser({
        userName,
        claimName: 'quyen',
        value,
      });
      res && newChecked.push(value);
    } else {
      const res = await accountService.removeClaimToUser({
        value: checkedList[currentIndex],
        userName,
        claimName: 'quyen',
      });
      res && newChecked.splice(currentIndex, 1);
    }

    onChange(newChecked);
    setLoading(prev => ({...prev, [value]: false}));
  };

  // const onSubmit = (data: any) => {
  //
  // };
  // const handleDelete = async () => {

  //   const res = await accountService.removeClaimToRole({
  //     value:
  //     roleName: role?.name,
  //     claimName: 'quyen',
  //   });
  //   if (res) {
  //     getClaim();
  //   }
  // };

  return (
    <List
      sx={{width: '100%', maxWidth: 360, bgcolor: 'background.paper'}}
      subheader={
        <ListSubheader
          sx={{'&.MuiListSubheader-root': {lineHeight: 'normal', pl: 0}}}
          component="div"
        >
          {title}
        </ListSubheader>
      }
      disablePadding
    >
      {listQuyen.map((item, index) => {
        return (
          <ListItem key={index + 1} disablePadding>
            <ListItemIcon>
              {item === 'seen' ? (
                <Eye size="16" />
              ) : item === 'create' ? (
                <AddSquare size="16" />
              ) : item === 'edit' ? (
                <Edit size="16" />
              ) : (
                <Trash size="16" />
              )}
            </ListItemIcon>
            <ListItemText
              id={`switch-list-label-${`${id};${item}`}`}
              primary={
                item === 'seen'
                  ? 'Xem'
                  : item === 'create'
                  ? 'Tạo'
                  : item === 'edit'
                  ? 'Sửa'
                  : 'Xóa'
              }
            />
            <Switch
              disabled={loading[`${id};${item}`]}
              edge="end"
              onChange={handleToggle(`${id};${item}`)}
              checked={checkedList.indexOf(`${id};${item}`) !== -1}
              inputProps={{
                'aria-labelledby': `switch-list-label-${`${id};${item}`}`,
              }}
            />
          </ListItem>
        );
      })}

      {/* <ListItem disablePadding>
        <ListItemIcon></ListItemIcon>
        <ListItemText id="switch-list-label-bluetooth" primary="Tạo" />
        <Switch
          disabled={loading[`${id};create`]}
          edge="end"
          onChange={handleToggle(`${id};create`)}
          checked={checkedList.indexOf(`${id};create`) !== -1}
          inputProps={{
            'aria-labelledby': 'switch-list-label-bluetooth',
          }}
        />
      </ListItem>
      <ListItem disablePadding>
        <ListItemIcon>
          <Edit size="16" />
        </ListItemIcon>
        <ListItemText id="switch-list-label-bluetooth" primary="Sửa" />
        <Switch
          disabled={loading[`${id};edit`]}
          edge="end"
          onChange={handleToggle(`${id};edit`)}
          checked={checkedList.indexOf(`${id};edit`) !== -1}
          inputProps={{
            'aria-labelledby': 'switch-list-label-bluetooth',
          }}
        />
      </ListItem>
      <ListItem disablePadding>
        <ListItemIcon>
          <Trash size="16" />
        </ListItemIcon>
        <ListItemText id="switch-list-label-bluetooth" primary="Xóa" />
        <Switch
          disabled={loading[`${id};delete`]}
          edge="end"
          onChange={handleToggle(`${id};delete`)}
          checked={checkedList.indexOf(`${id};delete`) !== -1}
          inputProps={{
            'aria-labelledby': 'switch-list-label-bluetooth',
          }}
        />
      </ListItem> */}
    </List>
  );
};

export default DialogGanQuyen;
