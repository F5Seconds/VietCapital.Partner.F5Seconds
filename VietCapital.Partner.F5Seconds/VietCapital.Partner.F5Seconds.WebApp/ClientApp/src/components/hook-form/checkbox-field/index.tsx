import {Checkbox, FormControlLabel, FormGroup} from '@mui/material';
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
const CheckboxField: FC<Props> = props => {
  const {label, name, rules, form, onChange, ...rest} = props;
  const {
    control,
    // formState: {errors},
  } = form;
  return (
    <Controller
      name={name}
      control={control}
      rules={rules}
      render={({field}) => (
        <FormGroup>
          <FormControlLabel
            control={
              <Checkbox
                {...field}
                checked={field.value}
                onChange={e => {
                  field.onChange(e);
                  onChange && onChange(e.target.checked);
                }}
                {...rest}
              />
            }
            label={label}
          />
        </FormGroup>
      )}
    />
  );
};

export default CheckboxField;
