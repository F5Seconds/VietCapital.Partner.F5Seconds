import {DateRange, LocalizationProvider} from '@mui/lab';
import AdapterDate from '@mui/lab/AdapterMoment';
import DateRangePicker from '@mui/lab/DateRangePicker';
import {Box, Button, Stack, TextField} from '@mui/material';
import queryString from 'query-string';
import {useEffect, useState} from 'react';
import CsvDownloader from 'react-csv-downloader';
import {useLocation, useNavigate} from 'react-router-dom';
import {DataTable, SearchBar} from '../../../components/base';
import useCheckQuyen from '../../../hooks/useCheckQuyen';
import Page from '../../../layouts/Page';
import {PaginationParams, QueryParams, Transaction} from '../../../models';
import transactionService from '../../../services/transaction-service';
import {state, stateColor} from '../../../utils/state';
import DialogDetail from './dialog-detail';

const DanhSachDonHangPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const queryParams: QueryParams = queryString.parse(location.search);
  const [openDialog, setOpenDialog] = useState<{open: boolean; row?: any}>({
    open: false,
    row: null,
  });
  const [isLoading, setIsLoading] = useState(false);

  const [list, setList] = useState<Transaction[]>([]);
  const [dateRange, setDateRange] = useState<DateRange<Date>>([null, null]);

  const [filters, setFilters] = useState<QueryParams & {from?: Date; to?: Date}>({
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
      field: 'transactionId',
      headerName: 'Mã giao dịch',
    },
    {
      field: 'customerId',
      headerName: 'Mã khách hàng',
    },
    {field: 'productId', headerName: 'Mã sản phẩm'},

    {
      field: 'productName',
      headerName: 'Tên sản phẩm',
    },
    {
      field: 'productPoint',
      headerName: 'Điểm',
    },
    {
      field: 'voucherCode',
      headerName: 'Mã voucher',
    },
    {
      field: 'state',
      headerName: 'Trạng thái',
      renderCell: (row: any) => <Box sx={{color: stateColor(row.state)}}>{state(row.state)}</Box>,
    },
    {
      field: 'created',
      headerName: 'Ngày giao dịch',
      renderCell: (row: any) => new Date(row.created).toLocaleDateString('vi'),
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

  useEffect(() => {
    dateRange[0] &&
      dateRange[1] &&
      setFilters(prev => ({
        ...prev,
        from: dateRange[0] || undefined,
        to: dateRange[1] || undefined,
      }));
  }, [dateRange]);

  const datas: {
    transactionId: string;
    customerId: string;
    productId: string;
    productName: string;
    productPoint: string;
    voucherCode: string;
    state: string;
    created: string;
    expiryDate: string;
  }[] = list.map(item => ({
    transactionId: item.transactionId + '',
    customerId: item.customerId,
    productId: item.productId + '',
    productName: item.product.name,
    productPoint: item.product.point + '',
    voucherCode: item.voucherCode + '',
    state: item.state + '',
    created: new Date(item.created).toLocaleDateString('vi'),
    expiryDate: new Date(item.expiryDate).toLocaleDateString('vi'),
  }));

  const [checkQuyen] = useCheckQuyen();
  if (!checkQuyen('seen')) {
    navigate('/404');
  }
  return (
    <Page title="Danh sách đơn hàng">
      <Stack direction="row" alignItems="center" justifyContent="space-between" marginBottom={2}>
        <SearchBar onSubmit={value => setFilters(prev => ({...prev, search: value}))} />
        <LocalizationProvider dateAdapter={AdapterDate} locale={'vi'}>
          <DateRangePicker
            startText="Từ ngày"
            toolbarPlaceholder="dd/mm/yyyy"
            endText="Đến ngày"
            value={dateRange}
            inputFormat={'DD/MM/YYYY'}
            onChange={newValue => {
              setDateRange(newValue);
            }}
            renderInput={(startProps, endProps) => (
              <>
                <TextField {...startProps} margin="dense" InputLabelProps={{shrink: true}} />
                <Box sx={{mx: 2}}> Đến </Box>
                <TextField {...endProps} margin="dense" InputLabelProps={{shrink: true}} />
              </>
            )}
          />
        </LocalizationProvider>
        <CsvDownloader
          filename="myfile"
          extension=".csv"
          separator=";"
          wrapColumnChar="'"
          columns={columns.map(item => ({id: item.field, displayName: item.headerName}))}
          datas={datas}
        >
          <Button variant="contained">Tải xuống file CSV</Button>
        </CsvDownloader>
      </Stack>

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
            setFilters(prev => ({...prev, pageSize: value, pageNumber: 1}));
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
