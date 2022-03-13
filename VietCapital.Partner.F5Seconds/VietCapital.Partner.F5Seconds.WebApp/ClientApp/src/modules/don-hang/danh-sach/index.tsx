import {Box, Stack} from '@mui/material';
import queryString from 'query-string';
import React, {useEffect, useState} from 'react';
import {useLocation} from 'react-router';
import {DataTable, SearchBar} from '../../../components/base';
import Page from '../../../layouts/Page';
import {PaginationParams, QueryParams, Transaction} from '../../../models';
import transactionService from '../../../services/transaction-service';
import {state, stateColor} from '../../../utils/state';
import DialogDetail from './dialog-detail';

const DanhSachDonHangPage = () => {
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

        setList(res.data);
        setPagination({currentPage, pageSize, totalCount, totalPages, hasNext, hasPrevious});
      }
      setIsLoading(false);
    };
    getList();
  }, [filters]);

  return (
    <Page title="Danh sách đơn hàng">
      <Stack direction="row" justifyContent="space-between" marginBottom={2}>
        <SearchBar onSubmit={value => setFilters(prev => ({...prev, search: value}))} />
      </Stack>
      {/* <Stack direction="row" justifyContent="flex-end" marginBottom={2}>
          <Button
            variant="contained"
            color="success"
            onClick={() => {
              setOpenDialog(prev => ({open: true}));
            }}
          >
            Thêm sản phẩm
          </Button>
        </Stack> */}
      <DataTable
        columns={columns}
        rows={list.map(item => ({
          ...item,
          productCode: item.product.productCode,
          productName: item.product.name,
          productPoint: item.product.point,
        }))}
        loading={isLoading}
        onRowClick={row => {
          setOpenDialog(prev => ({...prev, open: true, row}));
        }}
        pagination={{
          show: true,
          page: pagination.currentPage - 1,
          totalCount: pagination.totalCount,
          rowsPerPage: pagination.pageSize,
          onPageChange: page => {
            setFilters(prev => ({...prev, pageNumber: page + 1}));
          },
          onRowsPerPageChange: value => {
            setFilters(prev => ({...prev, pageSize: value, pageNumber: 0}));
          },
        }}
      />

      {openDialog.open && (
        <DialogDetail open={openDialog.open} row={openDialog.row} onClose={handleCloseDialog} />
      )}
    </Page>
  );
};

export default DanhSachDonHangPage;
