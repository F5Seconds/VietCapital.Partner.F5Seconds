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
  getAllUsersByRole: (params: any): Promise<any> =>
    axiosClient.get('/account/getAllUsersByRole', {params}),
  addUsersToRole: (params: any, data: any): Promise<any> =>
    axiosClient.post('/account/addUsersToRole?roleName=' + params?.roleName, data),
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

  //claim
  addClaimToRole: (data: any): Promise<any> => axiosClient.post('/account/addClaimToRoles', data),
  getAllClaimsInRole: (params: any): Promise<any> =>
    axiosClient.get('/account/GetAllClaimsInRole', {params}),
};

export default accountApi;
