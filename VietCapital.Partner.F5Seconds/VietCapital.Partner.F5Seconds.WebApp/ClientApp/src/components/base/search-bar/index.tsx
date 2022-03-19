import {Card, InputBase, Paper} from '@mui/material';
import {SearchNormal} from 'iconsax-react';
import React, {FC, useRef, useState} from 'react';

interface Props {
  placeholder?: string;
  onSubmit: (value: string) => void;
}
const SearchBar: FC<Props> = props => {
  const {placeholder = 'Vui lòng nhập từ khóa', onSubmit} = props;
  const [search, setSearch] = useState('');
  const typingRef = useRef<any>();

  const handleSearchDebounce = (value: string) => {
    setSearch(value);
    if (typingRef.current) {
      clearTimeout(typingRef.current);
    }
    typingRef.current = setTimeout(() => {
      onSubmit && onSubmit(value);
    }, 300);
  };
  return (
    <Card
      sx={{
        p: '2px 10px 2px 0px',
        display: 'flex',
        alignItems: 'center',
        width: 300,
        borderRadius: '8px',
        boxShadow: 'rgb(145 158 171 / 24%) 0px 1px 2px 0px',
      }}
    >
      <InputBase
        sx={{ml: 1, flex: 1}}
        placeholder={placeholder}
        value={search}
        onChange={({target: {value}}) => handleSearchDebounce(value)}
      />
      <SearchNormal color="#555" size={18} />
    </Card>
  );
};

export default SearchBar;
