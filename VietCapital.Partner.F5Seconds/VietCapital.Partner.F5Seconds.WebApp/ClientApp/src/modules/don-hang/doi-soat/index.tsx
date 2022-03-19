import {Box, Button, Stack, styled} from '@mui/material';
import queryString from 'query-string';
import React, {useEffect, useState} from 'react';
import CSVReader from 'react-csv-reader';
import {useLocation} from 'react-router';
import {DataTable, SearchBar} from '../../../components/base';
import Page from '../../../layouts/Page';
import {PaginationParams, QueryParams, Transaction} from '../../../models';
import transactionService from '../../../services/transaction-service';
import {state, stateColor} from '../../../utils/state';
import moment from 'moment';

const Input = styled('input')({
  display: 'none',
});

const DoiSoatPage = () => {
  const location = useLocation();

  const queryParams: QueryParams = queryString.parse(location.search);
  const [openDialog, setOpenDialog] = useState<{open: boolean; row?: any}>({
    open: false,
    row: null,
  });
  const [isLoading, setIsLoading] = useState(false);

  const [list, setList] = useState<Transaction[]>([]);

  const [filters, setFilters] = useState<QueryParams>({
    ...queryParams,
    search: queryParams.search ?? '',
    pageNumber: queryParams.pageNumber ?? 1,
    pageSize: queryParams.pageSize ?? 10,
  });

  const [pagination, setPagination] = useState<PaginationParams>({
    currentPage: 1,
    totalPages: 1,
    pageSize: 10,
    totalCount: 0,
    hasPrevious: false,
    hasNext: false,
  });

  const handleCloseDialog = () => setOpenDialog(prev => ({...prev, open: false}));

  const columns = [
    {
      field: 'customerId',
      headerName: 'Mã khách hàng',
    },
    {field: 'productCode', headerName: 'Mã sản phẩm'},

    {
      field: 'productName',
      headerName: 'Tên sản phẩm',
    },
    {
      field: 'productPoint',
      headerName: 'Điểm',
    },
    {
      field: 'state',
      headerName: 'Trạng thái',
      renderCell: (row: any) => <Box sx={{color: stateColor(row.state)}}>{state(row.state)}</Box>,
    },
    {
      field: 'expiryDate',
      headerName: 'Ngày hết hạn',
      renderCell: (row: any) => new Date(row.expiryDate).toLocaleDateString('vi'),
    },
    // {
    //   field: '',
    //   headerName: '',
    //   renderCell: (row: any) => (
    //     <IconButton
    //       size="medium"
    //       color="error"
    //       onClick={e => {
    //         e.stopPropagation();
    //         setIsOpenDelete({visible: true, id: row.id});
    //       }}
    //     >
    //       <Trash color={colors.error} />
    //     </IconButton>
    //   ),
    // },
  ];

  useEffect(() => {
    const getList = async () => {
      setIsLoading(true);
      const res = await transactionService.getAll(filters);
      if (res) {
        const {currentPage, pageSize, totalCount, totalPages, hasNext, hasPrevious} = res;

        // setList(res.data);
        // setPagination({currentPage, pageSize, totalCount, totalPages, hasNext, hasPrevious});
      }
      setIsLoading(false);
    };
    getList();
  }, [filters]);

  return (
    <Page title="Đối soát đơn hàng">
      <Stack direction="row" justifyContent="space-between" marginBottom={2}>
        <SearchBar onSubmit={value => setFilters(prev => ({...prev, search: value}))} />
        <label htmlFor="upload">
          <CSVReader
            cssClass="csv-reader-input"
            onFileLoaded={(data, fileInfo, originalFile) =>
              setList(prev => {
                return data.map(item => {
                  let object: any = {};

                  // for(i in columns) {

                  // }
                  columns.forEach(i => {
                    if (i.headerName === 'Ngày hết hạn') {
                      Object.assign(object, {
                        [i.field]: moment(item[`${i.headerName}`], 'DD/MM/YYYY').toDate(),
                      });
                      return;
                    }
                    Object.assign(object, {[i.field]: item[`${i.headerName}`]});
                  });
                  console.log(object);
                  return object;
                });
                // .flatMap(item => item);
              })
            }
            onError={error => console.log(error)}
            parserOptions={{
              header: true,
              dynamicTyping: true,
              skipEmptyLines: true,
              transformHeader: header => header,
            }}
            inputId="upload"
            inputStyle={{display: 'none'}}
          />
          <Button component="span" variant="contained">
            Tải lên file CSV
          </Button>
        </label>
      </Stack>

      <DataTable
        columns={columns}
        rows={list}
        loading={isLoading}
        onRowClick={row => {
          setOpenDialog(prev => ({...prev, open: true, row}));
        }}
        pagination={{
          show: false,
          page: pagination.currentPage - 1,
          totalCount: pagination.totalCount,
          rowsPerPage: list.length,
          onPageChange: page => {
            setFilters(prev => ({...prev, pageNumber: page + 1}));
          },
          onRowsPerPageChange: value => {
            setFilters(prev => ({...prev, pageSize: value, pageNumber: 1}));
          },
        }}
      />

      {/* {openDialog.open && (
        <DialogDetail open={openDialog.open} row={openDialog.row} onClose={handleCloseDialog} />
      )} */}
    </Page>
  );
};

export default DoiSoatPage;
