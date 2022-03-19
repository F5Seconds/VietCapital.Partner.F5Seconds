import {SnackbarProvider, useSnackbar} from 'notistack';
import React, {useEffect} from 'react';
import {useRoutes} from 'react-router-dom';
import {useAppDispatch, useAppSelector} from './redux/hooks';
import {selectAlert, setHiddenAlert} from './redux/slice/alertSlice';
import {routes} from './routes';
import ThemeConfig from './theme';
import GlobalStyles from './theme/globalStyles';
// import './App.css';
import './theme/styles.css';

const Noti = () => {
  const {enqueueSnackbar, closeSnackbar} = useSnackbar();
  const alert = useAppSelector(selectAlert);
  console.log('====================================');
  console.log(alert);
  console.log('====================================');
  useEffect(() => {
    if (alert.open) {
      enqueueSnackbar(alert.message, {variant: alert.type, autoHideDuration: 3000});
    }
  }, [alert.open]);
  return null;
};

function App() {
  const dispatch = useAppDispatch();
  return (
    <ThemeConfig>
      <GlobalStyles />

      <SnackbarProvider
        maxSnack={3}
        onClose={(event, reason) => {
          dispatch(setHiddenAlert());
        }}
      >
        <div style={{minHeight: '100vh', backgroundColor: '#E3F1FD'}}>
          {useRoutes(routes)}
          <Noti />
        </div>
      </SnackbarProvider>
    </ThemeConfig>
  );
}

export default App;
