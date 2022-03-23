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
  firstName?: string;
  lastName?: string;
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
    setJWT: (state, action: PayloadAction<CounterState>) => {
      const {jwToken} = action.payload;
      const jwtDecode: any = jwToken && jwt_decode(jwToken);
      state.jwToken = jwToken;
      state.id = jwtDecode?.uid;
      state.roles = jwtDecode?.roles;
      state.userName = jwtDecode?.userName;
      state.quyen = jwtDecode?.quyen;
    },
    setAuth: (state, action: PayloadAction<CounterState>) => {
      const {email, id, isVerified, jwToken, refreshToken, roles, userName, firstName, lastName} =
        action.payload;
      const jwtDecode: any = jwToken && jwt_decode(jwToken);

      state.email = email || state.email;
      state.isVerified = isVerified;
      state.jwToken = jwToken || state.jwToken;
      state.id = jwtDecode?.uid || id || state.id;
      state.refreshToken = refreshToken || state.refreshToken;
      state.roles = jwtDecode?.roles || state.roles;
      state.userName = jwtDecode?.userName || userName || state.userName;
      state.quyen = jwtDecode?.quyen || state.quyen;
      state.firstName = firstName || state.firstName;
      state.lastName = lastName || state.lastName;
    },
    logout: state => {
      state.email = '';
      state.isVerified = false;
      state.jwToken = '';
      state.id = '';
      state.refreshToken = '';
      state.roles = [];
      state.userName = '';
      state.quyen = [];
      state.firstName = '';
      state.lastName = '';
    },
  },
});

export const {setAuth, setJWT, logout} = counterSlice.actions;

// Other code such as selectors can use the imported `RootState` type
export const selectIsVerified = (state: RootState) => state.auth.isVerified;
export const selectJWT = (state: RootState) => state.auth.jwToken;
export const selectQuyen = (state: RootState) => state.auth.quyen;
export const selectAuth = (state: RootState) => state.auth;

export default counterSlice.reducer;
