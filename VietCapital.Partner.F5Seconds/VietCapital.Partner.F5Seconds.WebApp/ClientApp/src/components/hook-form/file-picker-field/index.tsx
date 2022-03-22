import {IconButton, InputAdornment, TextField, Tooltip} from '@mui/material';
import {DirectSend} from 'iconsax-react';
import React, {FC, useRef} from 'react';
import {Controller} from 'react-hook-form';
import {colors} from '../../../theme';

interface Props {
  label: string;
  name: string;
  rules?: any;
  form: any;
  onChange?: (file: any) => void;
  variant?: any;
}
const FilePickerField: FC<Props> = props => {
  const {label, name, rules, form, onChange, variant, ...rest} = props;
  const {
    control,
    formState: {errors},
  } = form;
  const fileRef = useRef<any>();

  return (
    <Controller
      name={name}
      control={control}
      rules={rules}
      render={({field}) => (
        <>
          <input
            hidden
            type="file"
            ref={fileRef}
            accept="image/*"
            onChange={({target: {files, value}}) => {
              field.onChange(files?.[0]);
              // console.log(files?.[0], value);
              onChange && onChange(files?.[0]);
            }}
          />
          <TextField
            {...field}
            {...rest}
            onClick={() => fileRef.current.click()}
            value={field.value?.name || field.value || ''}
            id={name}
            label={label}
            variant={variant || 'standard'}
            margin="none"
            fullWidth
            disabled
            error={!!errors[name]}
            helperText={errors[name]?.message}
            sx={{
              cursor: 'pointer',
            }}
            InputProps={{
              endAdornment: (
                <Tooltip title="Chọn tập tin">
                  <InputAdornment position="end">
                    <IconButton onClick={() => fileRef.current.click()}>
                      <DirectSend color={colors.info} />
                    </IconButton>
                  </InputAdornment>
                </Tooltip>
              ),
            }}
          />
        </>
      )}
    />
  );
};

export default FilePickerField;
