import {
  CircularProgress,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  Box,
  Card,
  TableRowProps,
} from '@mui/material';
import React, {FC, useEffect, useLayoutEffect, useRef, useState} from 'react';

interface Column {
  field: string;
  headerName: string;
  type?: 'text' | 'number';
  valueGetter?: (row: any) => void;
  renderCell?: (row: any) => void;
  center?: boolean;
}
interface Props {
  columns: Array<Column>;
  rows: Array<any>;
  height?: number;
  pagination?: {
    rowsPerPage: number;
    page: number;
    onPageChange?: (page: number) => void; //page
    onRowsPerPageChange?: (value: any) => void; //value
    totalCount: number;
    show: boolean;
  };
  loading?: boolean;
  onRowClick: (item: any, index: number) => void;
}

const DataTable: FC<Props> = props => {
  const {
    columns,
    rows = [],
    height,
    pagination = {
      show: false,
      page: 0,
      rowsPerPage: 10,
    },
    loading = false,
    onRowClick,
  } = props;

  const [heightRow, setHeightRow] = useState(0);

  const heightRowRef = useRef<any>(null);

  useEffect(() => {
    setHeightRow(heightRowRef.current?.clientHeight || 0);
  });

  const emptyRows = pagination.page > 0 ? Math.max(0, pagination.rowsPerPage - rows.length) : 0;

  console.log(pagination, {emptyRows, heightRow, heightRowRef});

  return (
    <Card sx={{border: '1px solid rgba(0,0,0,0.2)', position: 'relative'}}>
      {loading && (
        <Stack
          justifyContent="center"
          alignItems="center"
          sx={{
            position: 'absolute',
            zIndex: 5,
            height: '100%',
            width: '100%',
          }}
        >
          <CircularProgress />
        </Stack>
      )}
      <TableContainer style={{overflowX: 'auto', minWidth: 800}}>
        <Table sx={{minWidth: 650}} stickyHeader>
          <TableHead>
            <TableRow>
              {columns.map((item: Column, index) => (
                <TableCell
                  align={item.center ? 'center' : item.type === 'number' ? 'right' : 'left'}
                  key={index.toString()}
                >
                  {item.headerName}
                </TableCell>
              ))}
            </TableRow>
          </TableHead>
          <TableBody>
            {rows.map((row, index) => (
              <TableRow
                ref={index === 0 ? heightRowRef : null}
                key={index.toString()}
                sx={{
                  '&:last-child td, &:last-child th': {border: 0},
                  cursor: !!onRowClick && 'pointer',
                }}
                onClick={() => onRowClick && onRowClick(row, index)}
              >
                {columns.map((item: Column) => (
                  <TableCell
                    key={item.field}
                    align={item.center ? 'center' : item.type === 'number' ? 'right' : 'left'}
                  >
                    {item.renderCell
                      ? item.renderCell(row)
                      : item.valueGetter
                      ? item?.valueGetter(row)
                      : row[item.field]}
                  </TableCell>
                ))}
              </TableRow>
            ))}
            {emptyRows > 0 && (
              <TableRow style={{height: heightRow * emptyRows}}>
                <TableCell colSpan={6} />
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
      {pagination.show && (
        <TablePagination
          rowsPerPageOptions={[5, 10, 25, 50, 100]}
          component="div"
          count={pagination.totalCount}
          rowsPerPage={pagination.rowsPerPage}
          page={pagination.page}
          labelDisplayedRows={({from, to, count}) =>
            `${from}–${to} trong ${count !== -1 ? count : `nhiều hơn ${to}`}`
          }
          labelRowsPerPage="Số hàng mỗi trang"
          onPageChange={(e, page) => pagination.onPageChange?.(page)}
          onRowsPerPageChange={({target: {value}}) => pagination.onRowsPerPageChange?.(value)}
        />
      )}
    </Card>
  );
};

export default DataTable;
