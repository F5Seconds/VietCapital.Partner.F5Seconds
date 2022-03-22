import {createSlice, PayloadAction} from '@reduxjs/toolkit';
import type {RootState} from '../store';
import jwt_decode from 'jwt-decode';

// Define a type for the slice state
interface CounterState {
  email?: string;
  id?: string;
  isVerified?: boolean;
  jwToken?: string;
  refreshToken?: string;
  roles?: string[];
  userName?: string;
  quyen?: string[];
}

// Define the initial state using that type
const initialState: CounterState = {
  email: '',
  id: '',
  isVerified: false,
  jwToken: '',
  refreshToken: '',
  roles: [],
  userName: '',
  quyen: [],
};

export const counterSlice = createSlice({
  name: 'auth',
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
    // Use the PayloadAction type to declare the contents of `action.payload`
    setAuth: (state, action: PayloadAction<CounterState>) => {
      const {email, id, isVerified, jwToken, refreshToken, roles, userName} = action.payload;
      const jwtDecode: any = jwToken && jwt_decode(jwToken);

      state.email = email;
      state.isVerified = isVerified;
      state.jwToken = jwToken;
      state.id = id;
      state.refreshToken = refreshToken;
      state.roles = jwtDecode?.roles;
      state.userName = jwtDecode?.userName;
      state.quyen = jwtDecode?.quyen;
    },
  },
});

export const {setAuth} = counterSlice.actions;

// Other code such as selectors can use the imported `RootState` type
export const selectIsVerified = (state: RootState) => state.auth.isVerified;
export const selectJWT = (state: RootState) => state.auth.jwToken;
export const selectQuyen = (state: RootState) => state.auth.quyen;

export default counterSlice.reducer;
