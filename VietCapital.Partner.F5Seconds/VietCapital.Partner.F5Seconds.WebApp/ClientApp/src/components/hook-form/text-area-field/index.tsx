import {TextareaAutosize} from '@mui/material';
import React, {FC} from 'react';
import {Controller} from 'react-hook-form';

interface Props {
  name: string;
  label: string;
  form: any;
  type?: any;
  [x: string]: any;
  rules?: any;
  onChange?: (value: string) => void;
}
const TextAreaField: FC<Props> = props => {
  const {label, name, rules, form, onChange, ...rest} = props;
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
        <TextareaAutosize
          {...field}
          {...rest}
          value={field.value || ''}
          onChange={e => {
            field.onChange(e);
            onChange && onChange(e.target.value);
          }}
          style={{width: '100%'}}
          id={name}
          minRows={5}
        />
      )}
    />
  );
};

export default TextAreaField;
