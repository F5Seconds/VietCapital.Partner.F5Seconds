using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Application.Wrappers;

namespace VietCapital.Partner.F5Seconds.Application.Features.Transactions.Commands
{
    public class CreateTransactionCommand : IRequest<Response<List<F5sVoucherCode>>>
    {
        public int productId { get; set; }
        public string productCode { get; set; }
        public int quantity { get; set; }
        public string transactionId { get; set; }
        public string customerId { get; set; }
        public string customerPhone { get; set; }
        public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Response<List<F5sVoucherCode>>>
        {
            private readonly IGatewayHttpClientService _gatewayHttpClient;
            private readonly IMapper _mapper;
            public CreateTransactionCommandHandler(IGatewayHttpClientService gatewayHttpClient, IMapper mapper)
            {
                _gatewayHttpClient = gatewayHttpClient;
                _mapper = mapper;
            }

            public async Task<Response<List<F5sVoucherCode>>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
            {
                var trans = await _gatewayHttpClient.BuyProduct(_mapper.Map<BuyVoucherPayload>(request));
                return trans;
            }
        }
    }
}
