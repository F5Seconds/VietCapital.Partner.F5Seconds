import {DateRange, DateRangePicker, LocalizationProvider} from '@mui/lab';
import AdapterMoment from '@mui/lab/AdapterMoment';
import {Box, Button, Card, Stack, styled, Tab, Tabs, TextField} from '@mui/material';
import {TickCircle} from 'iconsax-react';
import moment from 'moment';
import queryString from 'query-string';
import React, {useState} from 'react';
import CSVReader from 'react-csv-reader';
import {useLocation} from 'react-router';
import {DataTable, SearchBar} from '../../../components/base';
import Page from '../../../layouts/Page';
import {PaginationParams, QueryParams, Transaction} from '../../../models';
import {state, stateColor} from '../../../utils/state';

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
  const [dateRange, setDateRange] = useState<DateRange<Date>>([null, null]);
  const [tab, setTab] = React.useState(0);

  const handleChangeTab = (event: React.SyntheticEvent, newValue: number) => {
    setTab(newValue);
  };

  const renderTable = (list: any) => {
    return (
      <>
        <Stack direction="row" justifyContent="space-between" marginBottom={2}>
          <SearchBar onSubmit={value => setFilters(prev => ({...prev, search: value}))} />
        </Stack>
        <DataTable
          columns={columns}
          rows={list}
          loading={isLoading}
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
      </>
    );
  };
  return (
    <Page title="Đối soát đơn hàng">
      <Card sx={{p: 1, mb: 2}}>
        <Stack direction="row" alignItems="center" justifyContent="flex-start" spacing={2}>
          <LocalizationProvider dateAdapter={AdapterMoment} locale={'vi'}>
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
          <label htmlFor="upload">
            <CSVReader
              cssClass="csv-reader-input"
              onFileLoaded={(data, fileInfo, originalFile) =>
                setList(prev => {
                  return data.map(item => {
                    let object: any = {};
                    columns.forEach(i => {
                      if (i.headerName === 'Ngày hết hạn') {
                        Object.assign(object, {
                          [i.field]: moment(item[`${i.headerName}`], 'DD/MM/YYYY').toDate(),
                        });
                        return;
                      }
                      Object.assign(object, {[i.field]: item[`${i.headerName}`]});
                    });
                    return object;
                  });
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
            <Button
              component="span"
              variant="contained"
              startIcon={list.length ? <TickCircle size="16" variant="Bulk" /> : null}
            >
              Tải lên file CSV
            </Button>
          </label>
        </Stack>
      </Card>

      <Box sx={{borderBottom: 1, borderColor: 'divider'}}>
        <Tabs value={tab} onChange={handleChangeTab} aria-label="basic tabs example">
          <Tab label="Đối soát khớp" {...a11yProps(0)} />
          <Tab label="Đối soát không khớp F5S" {...a11yProps(1)} />
          <Tab label="Đối soát không khớp Bản Việt" {...a11yProps(2)} />
        </Tabs>
      </Box>
      <TabPanel value={tab} index={0}>
        {renderTable(list)}
      </TabPanel>
      <TabPanel value={tab} index={1}>
        Đối soát không khớp F5S
      </TabPanel>
      <TabPanel value={tab} index={2}>
        Đối soát không khớp Bản Việt
      </TabPanel>

      {/* {openDialog.open && (
        <DialogDetail open={openDialog.open} row={openDialog.row} onClose={handleCloseDialog} />
      )} */}
    </Page>
  );
};

function a11yProps(index: number) {
  return {
    id: `simple-tab-${index}`,
    'aria-controls': `simple-tabpanel-${index}`,
  };
}

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const {children, value, index, ...other} = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{py: 3}}>{children}</Box>}
    </div>
  );
}

export default DoiSoatPage;
