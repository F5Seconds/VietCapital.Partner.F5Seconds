using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetAllVoucherTransactions
{
    public class GetAllVoucherTransactionsQuery : IRequest<Response<object>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = "";
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

    }
    public class GetAllVoucherTransactionsQueryHandler : IRequestHandler<GetAllVoucherTransactionsQuery, Response<object>>
    {
        private readonly IVoucherTransactionRepositoryAsync _voucherTransactionRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllVoucherTransactionsQueryHandler(IVoucherTransactionRepositoryAsync voucherTransactionRepositoryAsync, IMapper mapper)
        {
            _voucherTransactionRepositoryAsync = voucherTransactionRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<object>> Handle(GetAllVoucherTransactionsQuery request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<GetAllVoucherTransactionsParameter>(request);
            var products = await _voucherTransactionRepositoryAsync.GetAllPagedListAsync(filter);
            if (products == null) return new Response<object>(false, null, ResponseConst.NotData);
            return new Response<object>(true, new
            {
                products.CurrentPage,
                products.TotalPages,
                products.PageSize,
                products.TotalCount,
                products.HasPrevious,
                products.HasNext,
                Data = _mapper.Map<ICollection<TransactionOutSideResponse>>(products),
            });
        }
    }
}
