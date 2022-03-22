import {Create} from '@mui/icons-material';
import {
  Card,
  CircularProgress,
  Divider,
  Grid,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  ListSubheader,
  Paper,
  Stack,
  Switch,
} from '@mui/material';
import {Add, AddSquare, Edit, Eye, Trash} from 'iconsax-react';
import React, {FC, useEffect, useState} from 'react';
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
  onSubmit: (data: any) => void;
}

const DialogGanQuyen: FC<Props> = ({open = false, id, onClose, onSubmit}) => {
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

  const role: any = watch('role');

  useEffect(() => {
    const getClaim = () => {
      accountService.getAllClaimsInRole({roleName: role?.name}).then(res => {
        console.log('====================================');
        // console.log(res);
        console.log('====================================');
        const list = res?.clams;
        list && setCheckedList(list);
      });
    };

    if (role) {
      accountService.getAllUsersByRole({roleId: role?.id}).then(res => {
        console.log('====================================');
        // console.log(res);
        console.log('====================================');
        const list = res?.listUser;
        const users = listUsers
          ?.filter((item: any) => list?.includes(item?.username))
          .map((item: any) => ({...item, label: item?.name, value: item?.email}));

        setValue<any>('user', users);
      });
      getClaim();
    }
  }, [role]);

  return (
    <DialogBase
      open={open}
      title="Gán quyền"
      onClose={onClose}
      textPositive="Gán quyền"
      onSubmit={handleSubmit(onSubmit)}
    >
      {isLoading ? (
        <Stack direction="row" justifyContent="center">
          <CircularProgress size={24} />
        </Stack>
      ) : (
        <Grid container spacing={2}>
          <Grid item xs={12}>
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
          </Grid>
          <Grid item xs={12}>
            <AutocompleteAsyncField
              form={form}
              name="user"
              label="Nhân viên *"
              items={listUsers?.map((item: any) => ({
                ...item,
                label: item.name,
                value: item.email,
              }))}
              multiple
              rules={{
                required: {
                  value: true,
                  message: 'Không được để trống',
                },
              }}
              loading={isLoadingRole}
            />
          </Grid>

          {role?.name && (
            <>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    roleName={role.name}
                    title="Danh sách user"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/quan-ly-user/danh-sach-user"
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    roleName={role.name}
                    title="Phân quyền user"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/quan-ly-user/phan-quyen-user"
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    roleName={role.name}
                    title="Danh mục"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/danh-muc"
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    roleName={role.name}
                    title="Sản phẩm"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/san-pham"
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    roleName={role.name}
                    title="Danh sách đơn hàng"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/don-hang/danh-sach-don-hang"
                  />
                </Card>
              </Grid>
              <Grid item xs={12} md={6} lg={4}>
                <Card sx={{p: 2}} variant="outlined">
                  <ListThaoTac
                    roleName={role.name}
                    title="Đối soát"
                    checkedList={checkedList}
                    onChange={setCheckedList}
                    id="/don-hang/doi-soat"
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
  roleName: string;
  title: string;
  checkedList: string[];
  onChange: (value: string[]) => void;
  id: string;
}

const ListThaoTac: FC<ListThaoTacProps> = ({roleName, checkedList, onChange, title, id}) => {
  const [loading, setLoading] = useState<{[x: string]: boolean}>({
    [`${id};seen`]: false,
    [`${id};edit`]: false,
  });
  const handleToggle = (value: string) => async () => {
    setLoading(prev => ({...prev, [value]: true}));

    const currentIndex = checkedList.indexOf(value);
    const newChecked = [...checkedList];

    if (currentIndex === -1) {
      const res = await accountService.addClaimToRole({
        roleName,
        claimName: 'quyen',
        value,
      });
      res && newChecked.push(value);
    } else {
      const res = await accountService.removeClaimToRole({
        value: checkedList[currentIndex],
        roleName,
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
      <ListItem disablePadding>
        <ListItemIcon>
          <Eye size="16" />
        </ListItemIcon>
        <ListItemText id="switch-list-label-wifi" primary="Xem" />
        <Switch
          disabled={loading[`${id};seen`]}
          edge="end"
          onChange={handleToggle(`${id};seen`)}
          checked={checkedList.indexOf(`${id};seen`) !== -1}
          inputProps={{
            'aria-labelledby': 'switch-list-label-wifi',
          }}
        />
      </ListItem>
      <ListItem disablePadding>
        <ListItemIcon>
          <Edit size="16" />
        </ListItemIcon>
        <ListItemText id="switch-list-label-bluetooth" primary="Thao tác" />
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
      {/* <ListItem disablePadding>
        <ListItemIcon>
          <Trash size="16" />
        </ListItemIcon>
        <ListItemText id="switch-list-label-bluetooth" primary="Xóa" />
        <Switch
          edge="end"
          onChange={handleToggle('delete')}
          checked={checked.indexOf('delete') !== -1}
          inputProps={{
            'aria-labelledby': 'switch-list-label-bluetooth',
          }}
        />
      </ListItem> */}
    </List>
  );
};

export default DialogGanQuyen;
