// material
import {CssBaseline} from '@mui/material';
import {createTheme, StyledEngineProvider, ThemeProvider} from '@mui/material/styles';
import PropTypes from 'prop-types';
import {ReactNode, useMemo} from 'react';
import componentsOverride from './overrides';
//
import palette from './palette';
import shadows, {customShadows, ICustomShadows} from './shadows';
import typography from './typography';

// ----------------------------------------------------------------------

ThemeConfig.propTypes = {
  children: PropTypes.node,
};

export const colors = {
  primary: '#024aac',
  primaryDark: '#022359',
  edit: '#697689',
  delete: '#f47373',
  info: '#0277BD',
  error: '#F44336',
  success: '#42B814',
  warning: '#F4A630',
  gray: '#697689',
};

declare module '@mui/material/styles' {
  interface Theme {
    status: {
      danger: React.CSSProperties['color'];
    };
  }

  interface Palette {
    neutral: Palette['primary'];
  }
  interface PaletteOptions {
    neutral?: PaletteOptions['primary'];
  }

  interface PaletteColor {
    darker?: string;
  }
  interface SimplePaletteColorOptions {
    darker?: string;
  }
  interface ThemeOptions {
    customShadows: ICustomShadows;
  }
}

interface ThemeConfigProps {
  children: ReactNode;
}

export default function ThemeConfig({children}: ThemeConfigProps) {
  const themeOptions = useMemo(
    () => ({
      palette,
      shape: {borderRadius: 8},
      typography,
      shadows,
      customShadows,
    }),
    []
  );

  const theme: any = createTheme(themeOptions);
  theme.components = componentsOverride(theme);

  return (
    <StyledEngineProvider injectFirst>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </ThemeProvider>
    </StyledEngineProvider>
  );
}
