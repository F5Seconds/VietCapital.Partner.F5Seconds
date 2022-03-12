import {colors} from '../theme';

export const state = (state: number): any => {
  const result: any = {
    1: 'Chưa sử dụng',
    2: 'Đã sử dụng',
    3: 'Hủy',
    4: 'Hết hạn sử dụng',
  };
  return result[state];
};

export const stateColor = (state: number): any => {
  const result: any = {
    1: colors.primary,
    2: colors.success,
    3: colors.error,
    4: colors.warning,
  };
  return result[state];
};
