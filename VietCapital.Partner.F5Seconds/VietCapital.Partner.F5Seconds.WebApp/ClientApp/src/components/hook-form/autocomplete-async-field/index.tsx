import {Autocomplete, CircularProgress, TextField} from '@mui/material';
import React, {FC, useRef, useState} from 'react';
import {Controller} from 'react-hook-form';

interface Props {
  name: string;
  label: string;
  form: any;
  type?: any;
  [x: string]: any;
  rules?: any;
  loading: boolean;
  placeholder?: string;
  onSubmit?: (value: string) => void;
  onChange?: (value: string) => void;
  multiple?: boolean;
  items: {label: string; value: string | number}[];
}

const AutocompleteAsyncField: FC<Props> = props => {
  const {
    label,
    name,
    rules,
    form,
    placeholder,
    items = [],
    loading = false,
    onSubmit,
    onChange,
    multiple = false,
    ...rest
  } = props;
  const {
    control,
    formState: {errors},
  } = form;
  const [search, setSearch] = useState('');
  const typingTimeoutRef = useRef<any>(null);

  const handleSearchDebounce = (value: string) => {
    setSearch(value);
    if (typingTimeoutRef.current) {
      clearTimeout(typingTimeoutRef.current);
    }
    typingTimeoutRef.current = setTimeout(() => {
      if (onSubmit) {
        onSubmit(value);
      }
    }, 300);
  };

  return (
    <Controller
      name={name}
      control={control}
      rules={rules}
      render={({field}) => (
        <Autocomplete
          {...field}
          {...rest}
          id={name}
          noOptionsText="Không có dữ liệu"
          options={items}
          multiple={multiple}
          inputValue={search}
          filterOptions={options => options?.filter(item => item) || []}
          onInputChange={(e, value, reason) => {
            multiple
              ? reason === 'input' && handleSearchDebounce(value)
              : handleSearchDebounce(value);
          }}
          value={field?.value || (multiple ? [] : '')}
          onChange={(e, newValue) => {
            field.onChange(newValue);
            onChange && onChange(newValue);
          }}
          getOptionLabel={option => option.label || ''}
          loading={loading}
          loadingText="Đợi tí..."
          fullWidth
          isOptionEqualToValue={(option, value) => option.value === value.value}
          renderInput={params => (
            <TextField
              {...params}
              label={label}
              variant="standard"
              margin="none"
              error={!!errors[name]}
              helperText={errors[name]?.message}
              InputProps={{
                ...params.InputProps,
                endAdornment: (
                  <>
                    {loading ? <CircularProgress color="inherit" size={20} /> : null}
                    {params.InputProps.endAdornment}
                  </>
                ),
              }}
            />
          )}
        />
      )}
    />
  );
};

export default AutocompleteAsyncField;
