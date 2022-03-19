import {IconButton, InputAdornment, TextField, Tooltip} from '@mui/material';
import {DirectSend} from 'iconsax-react';
import React, {FC, useState} from 'react';
import {Controller} from 'react-hook-form';
import {colors} from '../../../theme';
import DialogMediaUpload from '../../base/dialog-media-upload';

interface Props {
  label: string;
  name: string;
  rules?: any;
  form: any;
  onChange?: (file: any) => void;
  variant?: any;
}
const ImagePickerField: FC<Props> = props => {
  const {label, name, rules, form, onChange, variant, ...rest} = props;
  const {
    control,
    formState: {errors},
  } = form;
  const [open, setOpen] = useState(false);
  return (
    <Controller
      name={name}
      control={control}
      rules={rules}
      render={({field}) => (
        <>
          <TextField
            {...field}
            {...rest}
            onClick={() => {
              setOpen(true);
            }}
            value={field.value || ''}
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
                    <IconButton onClick={() => setOpen(true)}>
                      <DirectSend color={colors.info} />
                    </IconButton>
                  </InputAdornment>
                </Tooltip>
              ),
            }}
          />
          <DialogMediaUpload
            open={open}
            init={field.value}
            onClose={() => setOpen(false)}
            onSubmit={data => {
              field.onChange(data[0]);
              setOpen(false);
            }}
          />
        </>
      )}
    />
  );
};

export default ImagePickerField;
