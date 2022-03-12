import accountApi from '../apis/account-api';
import {Role} from '../models';
import {setShowAlert} from '../redux/slice/alertSlice';
import store from '../redux/store';

const accountService = {
  login: async (email: string, password: string) => {
    try {
      await accountApi.login(email, password);
    } catch (error) {}
  },
  getAllUser: async (): Promise<any[] | undefined> => {
    try {
      const res = await accountApi.getAllUser();
      return res;
    } catch (error) {
      console.log('Lỗi get all user');
    }
  },
  //role
  getAllRole: async (): Promise<Role[] | undefined> => {
    try {
      const res = await accountApi.getAllRole();
      return res;
    } catch (error) {
      console.log('Lỗi get all role');
    }
  },
  createRole: async (roleName: string): Promise<boolean> => {
    try {
      const res = await accountApi.createRole(roleName);
      if (res.result) {
        store.dispatch(setShowAlert({message: res.result, type: 'success'}));
        return true;
      } else {
        store.dispatch(setShowAlert({message: res.error || '', type: 'error'}));
      }
    } catch (error) {
      console.log('Lỗi tạo role');
    }
    return false;
  },
  updateRole: async (roleId: string, roleName: string): Promise<boolean> => {
    try {
      const res = await accountApi.updateRole(roleId, roleName);
      if (res.result) {
        store.dispatch(setShowAlert({message: res.result, type: 'success'}));
        return true;
      }
    } catch (error) {
      store.dispatch(setShowAlert({message: 'Đã xảy ra lỗi', type: 'error'}));
    }
    return false;
  },
  deleteRole: async (roleId: string): Promise<boolean> => {
    try {
      const res = await accountApi.deleteRole(roleId);
      if (res.result) {
        store.dispatch(setShowAlert({message: res.result, type: 'success'}));
        return true;
      } else {
        store.dispatch(setShowAlert({message: res.error || '', type: 'error'}));
      }
    } catch (error) {
      store.dispatch(setShowAlert({message: 'Đã xảy ra lỗi', type: 'error'}));
    }
    return false;
  },
};

export default accountService;
