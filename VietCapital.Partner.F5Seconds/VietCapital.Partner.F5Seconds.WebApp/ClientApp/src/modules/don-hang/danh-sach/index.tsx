import {Box} from '@mui/material';
import queryString from 'query-string';
import React, {useEffect, useState} from 'react';
import {useLocation} from 'react-router';
import {DataTable} from '../../../components/base';
import {useWindowDimensions} from '../../../hooks';
import Header from '../../../layouts/Header';
import {PaginationParams, QueryParams, Transaction} from '../../../models';
import transactionService from '../../../services/transaction-service';
import {state, stateColor} from '../../../utils/state';
import DialogDetail from './dialog-detail';

const data = [
  {
    productId: 1043,
    transactionId: 'a5bbdbc2-1c34-4911-8e0e-3ce08998b017',
    productPrice: 20000,
    customerId: '123456',
    customerPhone: null,
    voucherCode: '9023953490',
    state: 2,
    expiryDate: '2022-06-10T00:00:00',
    usedTime: null,
    usedBrand: '',
    product: {
      id: 1043,
      productCode: 'F5S.NODODG',
      name: 'Quà tặng Got It (new)',
      content:
        'L&agrave; Tập đo&agrave;n sở hữu nhiều thương hiệu nh&agrave; h&agrave;ng nổi tiếng, Golden Gate Restaurant Group ti&ecirc;n phong &aacute;p dụng \\n m&ocirc; h&igrave;nh chuỗi nh&agrave; h&agrave;ng v&agrave; cam kết mang đến cho kh&aacute;ch h&agrave;ng những trải nghiệm tốt nhất từ những m&oacute;n ăn ngon đến chất lượng dịch vụ chuy&ecirc;n nghiệp.</p>',
      term: '<p>- Phiếu quà tặng điện tử được cung cấp bởi Got It.</p><p>- Một hoá đơn có thể sử dụng nhiều Phiếu quà tặng, mỗi Phiếu quà tặng có giá trị sử dụng 01 lần theo thời hạn quy định.</p><p>- Vui lòng xuất trình Phiếu quà tặng trước khi thực hiện thanh toán.</p><p>- Phiếu quà tặng có thể áp dụng cho các sản phẩm hoặc dịch vụ khác cùng giá trị khi sản phẩm/dịch vụ trên Phiếu quà tặng đã hết hàng.</p><p>- Phiếu quà tặng không có giá trị quy đổi thành tiền mặt hoặc hoàn lại tiền hoặc bán lại. Khách hàng sẽ trả thêm tiền nếu sử dụng quá giá trị Phiếu quà tặng.</p><p>- Khách hàng tự bảo mật thông tin mã quà tặng sau khi mua.</p><p>- Mã quà tặng sẽ không được hoàn lại khi bị mất hoặc Đã sử dụng.</p><p>- Khách hàng vui lòng liên hệ với nhà cung cấp khi có thắc mắc hoặc khiếu nại liên quan đến chất lượng của sản phẩm/dịch vụ theo hotline 1900558820 hoặc support@gotit.vn.</p>',
      image: 'https://img-stg.gotit.vn/compress/580x580/2018/02/1518432100_go0gN.png',
      thumbnail: 'https://img-stg.gotit.vn/compress/580x580/2018/02/1518432100_go0gN.png',
      point: 5671,
      type: 1,
      partner: 'GOTIT',
      brandName: 'Got It_Multi ',
      brandLogo: 'https://img-stg.gotit.vn/compress/brand/1640603516_ooYct.png',
    },
  },
  {
    productId: 1043,
    transactionId: 'a5bbdbc2-1c34-4911-8e0e-3ce08998b017',
    productPrice: 20000,
    customerId: '123456',
    customerPhone: null,
    voucherCode: '9023953490',
    state: 1,
    expiryDate: '2022-06-10T00:00:00',
    usedTime: null,
    usedBrand: '',
    product: {
      id: 1043,
      productCode: 'F5S.NODODG',
      name: 'Quà tặng Got It (new)',
      content:
        'L&agrave; Tập đo&agrave;n sở hữu nhiều thương hiệu nh&agrave; h&agrave;ng nổi tiếng, Golden Gate Restaurant Group ti&ecirc;n phong &aacute;p dụng \\n m&ocirc; h&igrave;nh chuỗi nh&agrave; h&agrave;ng v&agrave; cam kết mang đến cho kh&aacute;ch h&agrave;ng những trải nghiệm tốt nhất từ những m&oacute;n ăn ngon đến chất lượng dịch vụ chuy&ecirc;n nghiệp.</p>',
      term: '<p>- Phiếu quà tặng điện tử được cung cấp bởi Got It.</p><p>- Một hoá đơn có thể sử dụng nhiều Phiếu quà tặng, mỗi Phiếu quà tặng có giá trị sử dụng 01 lần theo thời hạn quy định.</p><p>- Vui lòng xuất trình Phiếu quà tặng trước khi thực hiện thanh toán.</p><p>- Phiếu quà tặng có thể áp dụng cho các sản phẩm hoặc dịch vụ khác cùng giá trị khi sản phẩm/dịch vụ trên Phiếu quà tặng đã hết hàng.</p><p>- Phiếu quà tặng không có giá trị quy đổi thành tiền mặt hoặc hoàn lại tiền hoặc bán lại. Khách hàng sẽ trả thêm tiền nếu sử dụng quá giá trị Phiếu quà tặng.</p><p>- Khách hàng tự bảo mật thông tin mã quà tặng sau khi mua.</p><p>- Mã quà tặng sẽ không được hoàn lại khi bị mất hoặc Đã sử dụng.</p><p>- Khách hàng vui lòng liên hệ với nhà cung cấp khi có thắc mắc hoặc khiếu nại liên quan đến chất lượng của sản phẩm/dịch vụ theo hotline 1900558820 hoặc support@gotit.vn.</p>',
      image: 'https://img-stg.gotit.vn/compress/580x580/2018/02/1518432100_go0gN.png',
      thumbnail: 'https://img-stg.gotit.vn/compress/580x580/2018/02/1518432100_go0gN.png',
      point: 5671,
      type: 1,
      partner: 'GOTIT',
      brandName: 'Got It_Multi ',
      brandLogo: 'https://img-stg.gotit.vn/compress/brand/1640603516_ooYct.png',
    },
  },
  {
    productId: 1043,
    transactionId: 'a5bbdbc2-1c34-4911-8e0e-3ce08998b017',
    productPrice: 20000,
    customerId: '123456',
    customerPhone: null,
    voucherCode: '9023953490',
    state: 3,
    expiryDate: '2022-06-10T00:00:00',
    usedTime: null,
    usedBrand: '',
    product: {
      id: 1043,
      productCode: 'F5S.NODODG',
      name: 'Quà tặng Got It (new)',
      content:
        'L&agrave; Tập đo&agrave;n sở hữu nhiều thương hiệu nh&agrave; h&agrave;ng nổi tiếng, Golden Gate Restaurant Group ti&ecirc;n phong &aacute;p dụng \\n m&ocirc; h&igrave;nh chuỗi nh&agrave; h&agrave;ng v&agrave; cam kết mang đến cho kh&aacute;ch h&agrave;ng những trải nghiệm tốt nhất từ những m&oacute;n ăn ngon đến chất lượng dịch vụ chuy&ecirc;n nghiệp.</p>',
      term: '<p>- Phiếu quà tặng điện tử được cung cấp bởi Got It.</p><p>- Một hoá đơn có thể sử dụng nhiều Phiếu quà tặng, mỗi Phiếu quà tặng có giá trị sử dụng 01 lần theo thời hạn quy định.</p><p>- Vui lòng xuất trình Phiếu quà tặng trước khi thực hiện thanh toán.</p><p>- Phiếu quà tặng có thể áp dụng cho các sản phẩm hoặc dịch vụ khác cùng giá trị khi sản phẩm/dịch vụ trên Phiếu quà tặng đã hết hàng.</p><p>- Phiếu quà tặng không có giá trị quy đổi thành tiền mặt hoặc hoàn lại tiền hoặc bán lại. Khách hàng sẽ trả thêm tiền nếu sử dụng quá giá trị Phiếu quà tặng.</p><p>- Khách hàng tự bảo mật thông tin mã quà tặng sau khi mua.</p><p>- Mã quà tặng sẽ không được hoàn lại khi bị mất hoặc Đã sử dụng.</p><p>- Khách hàng vui lòng liên hệ với nhà cung cấp khi có thắc mắc hoặc khiếu nại liên quan đến chất lượng của sản phẩm/dịch vụ theo hotline 1900558820 hoặc support@gotit.vn.</p>',
      image: 'https://img-stg.gotit.vn/compress/580x580/2018/02/1518432100_go0gN.png',
      thumbnail: 'https://img-stg.gotit.vn/compress/580x580/2018/02/1518432100_go0gN.png',
      point: 5671,
      type: 1,
      partner: 'GOTIT',
      brandName: 'Got It_Multi ',
      brandLogo: 'https://img-stg.gotit.vn/compress/brand/1640603516_ooYct.png',
    },
  },
  {
    productId: 1043,
    transactionId: 'a5bbdbc2-1c34-4911-8e0e-3ce08998b017',
    productPrice: 20000,
    customerId: '123456',
    customerPhone: null,
    voucherCode: '9023953490',
    state: 4,
    expiryDate: '2022-06-10T00:00:00',
    usedTime: null,
    usedBrand: '',
    product: {
      id: 1043,
      productCode: 'F5S.NODODG',
      name: 'Quà tặng Got It (new)',
      content:
        'L&agrave; Tập đo&agrave;n sở hữu nhiều thương hiệu nh&agrave; h&agrave;ng nổi tiếng, Golden Gate Restaurant Group ti&ecirc;n phong &aacute;p dụng \\n m&ocirc; h&igrave;nh chuỗi nh&agrave; h&agrave;ng v&agrave; cam kết mang đến cho kh&aacute;ch h&agrave;ng những trải nghiệm tốt nhất từ những m&oacute;n ăn ngon đến chất lượng dịch vụ chuy&ecirc;n nghiệp.</p>',
      term: '<p>- Phiếu quà tặng điện tử được cung cấp bởi Got It.</p><p>- Một hoá đơn có thể sử dụng nhiều Phiếu quà tặng, mỗi Phiếu quà tặng có giá trị sử dụng 01 lần theo thời hạn quy định.</p><p>- Vui lòng xuất trình Phiếu quà tặng trước khi thực hiện thanh toán.</p><p>- Phiếu quà tặng có thể áp dụng cho các sản phẩm hoặc dịch vụ khác cùng giá trị khi sản phẩm/dịch vụ trên Phiếu quà tặng đã hết hàng.</p><p>- Phiếu quà tặng không có giá trị quy đổi thành tiền mặt hoặc hoàn lại tiền hoặc bán lại. Khách hàng sẽ trả thêm tiền nếu sử dụng quá giá trị Phiếu quà tặng.</p><p>- Khách hàng tự bảo mật thông tin mã quà tặng sau khi mua.</p><p>- Mã quà tặng sẽ không được hoàn lại khi bị mất hoặc Đã sử dụng.</p><p>- Khách hàng vui lòng liên hệ với nhà cung cấp khi có thắc mắc hoặc khiếu nại liên quan đến chất lượng của sản phẩm/dịch vụ theo hotline 1900558820 hoặc support@gotit.vn.</p>',
      image: 'https://img-stg.gotit.vn/compress/580x580/2018/02/1518432100_go0gN.png',
      thumbnail: 'https://img-stg.gotit.vn/compress/580x580/2018/02/1518432100_go0gN.png',
      point: 5671,
      type: 1,
      partner: 'GOTIT',
      brandName: 'Got It_Multi ',
      brandLogo: 'https://img-stg.gotit.vn/compress/brand/1640603516_ooYct.png',
    },
  },
];
const DanhSachDonHangPage = () => {
  const location = useLocation();

  const queryParams: QueryParams = queryString.parse(location.search);
  const [openDialog, setOpenDialog] = useState<{open: boolean; row?: any}>({
    open: false,
    row: null,
  });
  const [isLoading, setIsLoading] = useState(false);

  const [list, setList] = useState<Transaction[]>([]);

  const {height} = useWindowDimensions();

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
    <div>
      <Header title="Quản lý đơn hàng" />
      <div style={{padding: 16}}>
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
          height={height - 200}
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
      </div>
      {openDialog.open && (
        <DialogDetail open={openDialog.open} row={openDialog.row} onClose={handleCloseDialog} />
      )}
    </div>
  );
};

export default DanhSachDonHangPage;
