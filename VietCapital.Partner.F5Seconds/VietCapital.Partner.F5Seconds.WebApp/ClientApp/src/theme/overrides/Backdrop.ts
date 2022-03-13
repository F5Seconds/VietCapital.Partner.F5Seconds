import {alpha} from '@mui/material/styles';
import {ThemeOptions} from '@mui/material';
// ----------------------------------------------------------------------

export default function Backdrop(theme: ThemeOptions) {
  const varLow = alpha(theme?.palette?.grey?.[900] || '', 0.48);
  const varHigh = alpha(theme?.palette?.grey?.[900] || '', 1);

  return {
    MuiBackdrop: {
      styleOverrides: {
        root: {
          background: `#00000050`,
          backdropFilter: 'blur(2px)',
          '&.MuiBackdrop-invisible': {
            background: 'transparent',
          },
        },
      },
    },
  };
}
