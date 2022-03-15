import {TextField} from '@mui/material';
import React, {FC} from 'react';
import {Controller} from 'react-hook-form';

interface Props {
  name: string;
  label: string;
  form: any;
  type?: any;
  [x: string]: any;
  rules?: any;
}

const InputField: FC<Props> = ({name, rules, label, form, type, ...rest}) => {
  const {
    control,
    formState: {errors},
  } = form;

  return (
    <Controller
      name={name}
      control={control}
      rules={rules}
      render={({field}) => (
        <TextField
          {...field}
          {...rest}
          id={name}
          label={label}
          variant="standard"
          margin="none"
          type={type}
          fullWidth
          error={!!errors[name]}
          helperText={errors[name]?.message}
          InputLabelProps={{shrink: true}}
        />
      )}
    />
  );
};

export default InputField;
