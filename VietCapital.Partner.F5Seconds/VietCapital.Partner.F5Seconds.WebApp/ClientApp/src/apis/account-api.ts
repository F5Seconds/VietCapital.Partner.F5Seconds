import {Account, Role} from '../models';
import axiosClient from './axiosClient';
import queryString from 'query-string';
const accountApi = {
  login: (email: string, password: string): Promise<any> =>
    axiosClient.post('/account/authenticate', {
      email,
      password,
    }),
  register: (data: Account): Promise<any> => axiosClient.post('/account/register', data),
  getAllUser: (): Promise<any> => axiosClient.get('/account/getAllUser'),
  getUserById: (id: string | number | null): Promise<any> =>
    axiosClient.get(`/account/getUserById?id=${id}`),
  updateUser: (id: string | number | null, data: Partial<Account>): Promise<any> =>
    axiosClient.put(`/account/updateUser?id=${id}`, data),
  removeUser: (params: {userName: string}): Promise<any> =>
    axiosClient.delete(`/account/removeUser?${queryString.stringify(params)}`),
  getAllUsersByRole: (params: any): Promise<any> =>
    axiosClient.get('/account/getAllUsersByRole', {params}),
  addUsersToRole: (params: any, data: any): Promise<any> =>
    axiosClient.post('/account/addUsersToRole?roleName=' + params?.roleName, data),
  addUserToRole: (params: {userName: string; rolename: string}): Promise<any> =>
    axiosClient.post(`/account/addUserToRole?${queryString.stringify(params)}`),
  removeUserFromRole: (params: {userName: string; rolename: string}): Promise<any> =>
    axiosClient.delete(`/account/removeUserFromRole?${queryString.stringify(params)}`),
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

  //claim role
  addClaimToRole: (params: {roleName: string; claimName: string; value: string}): Promise<any> =>
    axiosClient.post(`/account/addClaimToRoles?${queryString.stringify(params)}`),
  getAllClaimsInRole: (params: any): Promise<{clams: string[]}> =>
    axiosClient.get('/account/GetAllClaimsInRole', {params}),
  removeClaimToRole: (params: {roleName: string; claimName: string; value: string}): Promise<any> =>
    axiosClient.delete(`/account/removeClaimToRole?${queryString.stringify(params)}`),

  // claim user
  addClaimToUser: (params: {userName: string; claimName: string; value: string}): Promise<any> =>
    axiosClient.post(`/account/addClaimToUser?${queryString.stringify(params)}`),
  getAllClaimsByUser: (params: {userName: string}): Promise<{type: string; value: string}[]> => {
    return axiosClient.get(`/account/GetAllClaimByUser?userName=${params.userName}`);
  },
  removeClaimToUser: (params: {userName: string; claimName: string; value: string}): Promise<any> =>
    axiosClient.delete(`/account/removeClaimToUser?${queryString.stringify(params)}`),
};

export default accountApi;
