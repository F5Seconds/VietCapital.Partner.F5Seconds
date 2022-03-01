import {axiosClient} from './axiosClient';
const accountApi = {
  login: (email: string, password: string) =>
    axiosClient.post('/account/authenticate', {
      email,
      password,
    }),
};
export default accountApi;
