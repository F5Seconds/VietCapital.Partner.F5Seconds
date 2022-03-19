using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;

namespace VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter
{
    public class GetVoucherTransFilterQuery : IRequest<Response<object>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = "";
        public string Cif { get; set; }
        public int State { get; set; }
        public class GetVoucherTransFilterQueryHandler : IRequestHandler<GetVoucherTransFilterQuery, Response<object>>
        {
            private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
            private readonly IMapper _mapper;
            public GetVoucherTransFilterQueryHandler(IVoucherTransactionRepositoryAsync voucherTransaction, IMapper mapper)
            {
                _voucherTransaction = voucherTransaction;
                _mapper = mapper;
            }

            public async Task<Response<object>> Handle(GetVoucherTransFilterQuery request, CancellationToken cancellationToken)
            {
                var filter = _mapper.Map<GetVoucherTransFilterParameter>(request);
                var trans = await _voucherTransaction.GetPagedVoucherTransByFilter(filter);
                if (trans is null) return new Response<object>(false,null, ResponseConst.NotData);
                return new Response<object>(true, new
                {
                    trans.CurrentPage,
                    trans.TotalPages,
                    trans.PageSize,
                    trans.TotalCount,
                    trans.HasPrevious,
                    trans.HasNext,
                    Data = _mapper.Map<ICollection<TransactionOutSideResponse>>(trans)
                });
            }
        }
    }
}
