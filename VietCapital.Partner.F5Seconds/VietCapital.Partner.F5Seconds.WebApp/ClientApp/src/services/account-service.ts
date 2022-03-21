import accountApi from '../apis/account-api';
import {Account, Role} from '../models';
import {setShowAlert} from '../redux/slice/alertSlice';
import store from '../redux/store';

const accountService = {
  login: async (email: string, password: string) => {
    try {
      await accountApi.login(email, password);
    } catch (error) {}
  },
  getAllUser: async (): Promise<any> => {
    try {
      const res = await accountApi.getAllUser();
      return res;
    } catch (error) {
      console.log('Lỗi get all user');
    }
  },
  getAllUsersByRole: async (params: any): Promise<any> => {
    try {
      const res = await accountApi.getAllUsersByRole(params);
      return res;
    } catch (error) {
      console.log('Lỗi get all user');
    }
  },
  getUserById: async (id: string | number | null): Promise<any> => {
    try {
      const res = await accountApi.getUserById(id);
      return res;
    } catch (error) {
      console.log('Lỗi get user');
    }
  },
  register: async (data: Account): Promise<any> => {
    try {
      const res = await accountApi.register(data);
      store.dispatch(setShowAlert({message: 'Thêm user thành công', type: 'success'}));
      return res;
    } catch (error) {
      store.dispatch(setShowAlert({message: 'Đã xảy ra lỗi', type: 'error'}));
      console.log('Lỗi get user');
    }
  },
  updateUser: async (id: string | number | null, data: any): Promise<any> => {
    try {
      const res = await accountApi.updateUser(id, data);
      store.dispatch(setShowAlert({message: 'Cập nhật user thành công', type: 'success'}));
      return res;
    } catch (error) {
      store.dispatch(setShowAlert({message: 'Đã xảy ra lỗi', type: 'error'}));
      console.log('Lỗi get all user');
    }
  },

  addUsersToRole: async (params: any, data: any): Promise<any> => {
    try {
      const res = await accountApi.addUsersToRole(params, data);
      if (res.result) {
        store.dispatch(setShowAlert({message: res.result, type: 'success'}));
        return true;
      } else {
        store.dispatch(setShowAlert({message: res.error || '', type: 'error'}));
      }
    } catch (error) {
      console.log('Lỗi gán quyền');
    }
    return false;
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
  addClaimToRole: async (data: {
    roleName: string;
    claimName: string;
    value: string;
  }): Promise<any> => {
    try {
      const res = await accountApi.addClaimToRole(data);
      if (res.result) {
        store.dispatch(setShowAlert({message: res.result, type: 'success'}));
        return true;
      } else {
        store.dispatch(setShowAlert({message: res.error || '', type: 'error'}));
      }
    } catch (error) {
      console.log('Lỗi gán quyền');
    }
    return false;
  },
  getAllClaimsInRole: async (params: {
    roleName: string;
  }): Promise<{clams: string[]} | undefined> => {
    try {
      const res = await accountApi.getAllClaimsInRole(params);
      return res;
    } catch (error) {
      console.log('Lỗi get all user');
    }
  },
  removeClaimToRole: async (data: {
    roleName: string;
    claimName: string;
    value: string;
  }): Promise<any> => {
    try {
      const res = await accountApi.removeClaimToRole(data);
      if (res.result) {
        store.dispatch(setShowAlert({message: res.result, type: 'success'}));
        return true;
      } else {
        store.dispatch(setShowAlert({message: res.error || '', type: 'error'}));
      }
    } catch (error) {
      console.log('Lỗi gán quyền');
    }
    return false;
  },
};

export default accountService;
