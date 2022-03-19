using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetDoiSoatTransaction
{
    public class GetDoiSoatTransactionQuery : IRequest<Response<GetDoiSoatTransactionViewModel>>
    {
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public class GetAllVoucherTransactionsQueryHandler : IRequestHandler<GetDoiSoatTransactionQuery, Response<GetDoiSoatTransactionViewModel>>
        {
            private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
            public GetAllVoucherTransactionsQueryHandler(IVoucherTransactionRepositoryAsync voucherTransaction)
            {
                _voucherTransaction = voucherTransaction;
            }
            public async Task<Response<GetDoiSoatTransactionViewModel>> Handle(GetDoiSoatTransactionQuery request, CancellationToken cancellationToken)
            {
                return new Response<GetDoiSoatTransactionViewModel>(true, new GetDoiSoatTransactionViewModel()
                {
                    DoiSoatKhop = await _voucherTransaction.DoiSoatGiaoDichKhop(request.NgayBatDau, request.NgayKetThuc),
                    DoiSoatKhongKhopBvb = await _voucherTransaction.DoiSoatGiaoDichKhongKhopBvb(request.NgayBatDau,request.NgayKetThuc),
                    DoiSoatKhongKhopF5s = await _voucherTransaction.DoiSoatGiaoDichKhongKhopF5s(request.NgayBatDau,request.NgayKetThuc)
                });
            }
        }
    }
}
