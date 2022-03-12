import {Account, ResponseData, Role} from '../models';
import axiosClient from './axiosClient';

const accountApi = {
  login: (email: string, password: string): Promise<any> =>
    axiosClient.post('/account/authenticate', {
      email,
      password,
    }),
  register: (data: Account): Promise<any> => axiosClient.post('/account/register', data),
  getAllUser: (): Promise<any> => axiosClient.get('/account/getAllUser'),
  //role
  getAllRole: (): Promise<Role[]> => axiosClient.get('/account/role'),
  createRole: (roleName: string): Promise<{result?: string; error?: string}> => {
    const url = `/account/role?roleName=${roleName}`;
    return axiosClient.post(url);
  },
  deleteRole: (roleId: string): Promise<{result?: string; error?: string}> => {
    const url = `/account/deleteRole?roleId=${roleId}`;
    return axiosClient.delete(url);
  },
  updateRole: (roleId: string, roleName: string): Promise<{result?: string}> => {
    const url = `/account/updateRole?roleId=${roleId}&roleName=${roleName}`;
    return axiosClient.put(url);
  },
};

export default accountApi;
