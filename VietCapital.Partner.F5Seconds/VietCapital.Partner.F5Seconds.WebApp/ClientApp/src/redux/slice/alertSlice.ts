import {createSlice, PayloadAction} from '@reduxjs/toolkit';
import type {RootState} from '../store';

// Define a type for the slice state
interface AlertState {
  open?: boolean;
  message: string;
  type: 'info' | 'error' | 'success' | 'warning';
}

// Define the initial state using that type
const initialState: AlertState = {
  open: false,
  message: '',
  type: 'info',
};

export const alertSlice = createSlice({
  name: 'alert',

  initialState,
  reducers: {
    setShowAlert: (state, action: PayloadAction<AlertState>) => {
      const {message, type} = action.payload;
      state.message = message;
      state.type = type;
      state.open = true;
    },
    setHiddenAlert: state => {
      state.open = false;
      state.message = '';
      state.type = 'info';
    },
  },
});

export const {setHiddenAlert, setShowAlert} = alertSlice.actions;

// Other code such as selectors can use the imported `RootState` type
export const selectAlert = (state: RootState) => state.alert;

export default alertSlice.reducer;
