using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter
{
    public class GetVoucherTransFilterQuery : IRequest<Response<IReadOnlyList<VoucherTransaction>>>
    {
        public string Cif { get; set; }
        public int State { get; set; }
        public class GetVoucherTransFilterQueryHandler : IRequestHandler<GetVoucherTransFilterQuery, Response<IReadOnlyList<VoucherTransaction>>>
        {
            private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
            public GetVoucherTransFilterQueryHandler(IVoucherTransactionRepositoryAsync voucherTransaction)
            {
                _voucherTransaction = voucherTransaction;
            }
            public async Task<Response<IReadOnlyList<VoucherTransaction>>> Handle(GetVoucherTransFilterQuery request, CancellationToken cancellationToken)
            {
                var trans = await _voucherTransaction.GetVoucherTransactionByFilter(request.Cif,request.State);
                if (trans is null) return new Response<IReadOnlyList<VoucherTransaction>>(false,null, ResponseConst.NotData);
                return new Response<IReadOnlyList<VoucherTransaction>>(true,trans);
            }
        }
    }
}
