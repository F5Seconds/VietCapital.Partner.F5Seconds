using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Exceptions;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetVoucherTransactionById
{
    public class GetVoucherTransactionByIdQuery : IRequest<Response<VoucherTransaction>>
    {
        public int Id { get; set; }
    }
    public class GetVoucherTransactionByIdQueryHandler : IRequestHandler<GetVoucherTransactionByIdQuery, Response<VoucherTransaction>>
    {
        private readonly IVoucherTransactionRepositoryAsync _voucherTransactionRepositoryAsync;
        private readonly IMapper _mapper;
        public GetVoucherTransactionByIdQueryHandler(IVoucherTransactionRepositoryAsync voucherTransactionRepositoryAsync, IMapper mapper)
        {
            _voucherTransactionRepositoryAsync = voucherTransactionRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<VoucherTransaction>> Handle(GetVoucherTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var voucherTransaction = await _voucherTransactionRepositoryAsync.GetByIdAsync(request.Id);
            if (voucherTransaction == null)  throw new ApiException($"Category Not Found.");
            return new Response<VoucherTransaction>(voucherTransaction);
        }
    }
}
