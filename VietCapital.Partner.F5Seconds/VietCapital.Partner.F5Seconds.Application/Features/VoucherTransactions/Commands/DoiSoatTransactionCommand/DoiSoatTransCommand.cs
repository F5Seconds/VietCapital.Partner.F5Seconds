using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Commands.DoiSoatTransactionCommand
{
    public class DoiSoatTransCommand : IRequest<Response<DoiSoatTransactionViewModel>>
    {
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public List<DoiSoatTransInput> DoiSoatTrans { get; set; }
        public class DoiSoatTransCommandHandler : IRequestHandler<DoiSoatTransCommand, Response<DoiSoatTransactionViewModel>>
        {
            private readonly IVoucherTransactionBvbRepositoryAsync _voucherTransactionBvb;
            private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
            private readonly IMapper _mapper;
            public DoiSoatTransCommandHandler(
                IVoucherTransactionBvbRepositoryAsync voucherTransactionBvb,
                IVoucherTransactionRepositoryAsync voucherTransaction,
                IMapper mapper
                )
            {
                _voucherTransaction = voucherTransaction;
                _voucherTransactionBvb = voucherTransactionBvb;
                _mapper = mapper;
            }
            public async Task<Response<DoiSoatTransactionViewModel>> Handle(DoiSoatTransCommand request, CancellationToken cancellationToken)
            {
                await _voucherTransactionBvb.TruncateTable();
                await _voucherTransactionBvb.AddRangeAsync(_mapper.Map<List<VoucherTransactionsBvb>>(request.DoiSoatTrans));
                return new Response<DoiSoatTransactionViewModel>(true, new DoiSoatTransactionViewModel()
                {
                    DoiSoatKhop = _mapper.Map<List<DoiSoatTransOutput>>(await _voucherTransaction.DoiSoatGiaoDichKhop(request.NgayBatDau, request.NgayKetThuc)),
                    DoiSoatKhongKhopBvb = _mapper.Map<List<DoiSoatTransOutput>>(await _voucherTransaction.DoiSoatGiaoDichKhongKhopBvb(request.NgayBatDau, request.NgayKetThuc)),
                    DoiSoatKhongKhopF5s = _mapper.Map<List<DoiSoatTransOutput>>(await _voucherTransaction.DoiSoatGiaoDichKhongKhopF5s(request.NgayBatDau, request.NgayKetThuc))
                });
            }
        }
    }
}
