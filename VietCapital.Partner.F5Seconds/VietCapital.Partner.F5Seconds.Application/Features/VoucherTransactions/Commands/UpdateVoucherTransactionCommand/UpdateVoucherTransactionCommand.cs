using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Exceptions;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Commands.UpdateVoucherTransactionCommand
{
    public class UpdateVoucherTransactionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string TransactionId { get; set; }
        public float ProductPrice { get; set; }
        public string CustomerId { get; set; }
        public string CustomerPhone { get; set; }
        public string VoucherCode { get; set; }
        public int State { get; set; } = 1;
        public DateTime ExpiryDate { get; set; }
        public DateTime? UsedTime { get; set; }
        public string UsedBrand { get; set; }
        // public string  UsedBy { get; set; }
        public virtual Product Product { get; set; }

        public class UpdateVoucherTransactionCommandHandler : IRequestHandler<UpdateVoucherTransactionCommand, Response<int>>
        {
            private readonly IVoucherTransactionRepositoryAsync _voucherTransactionRepositoryAsync;
            private readonly IMapper _mapper;
            public UpdateVoucherTransactionCommandHandler(IVoucherTransactionRepositoryAsync voucherTransactionRepositoryAsync, IMapper mapper)
            {
                _voucherTransactionRepositoryAsync = voucherTransactionRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<int>> Handle(UpdateVoucherTransactionCommand command, CancellationToken cancellationToken)
            {
                var vouchertransaction = await _voucherTransactionRepositoryAsync.GetByIdAsync(command.Id);
                if (vouchertransaction == null)
                {
                    throw new ApiException($"VoucherTransaction Not Found.");
                }
                else
                {
                    vouchertransaction.ProductId = command.ProductId;
                    vouchertransaction.TransactionId = command.TransactionId;
                    vouchertransaction.ProductPrice = command.ProductPrice;
                    vouchertransaction.CustomerId = command.CustomerId;
                    vouchertransaction.CustomerPhone = command.CustomerPhone;
                    vouchertransaction.VoucherCode = command.VoucherCode;
                    vouchertransaction.State = command.State;
                    vouchertransaction.ExpiryDate = command.ExpiryDate;
                    vouchertransaction.UsedTime = command.UsedTime;
                    vouchertransaction.UsedBrand = command.UsedBrand;
                    vouchertransaction.Product = command.Product;
                    await _voucherTransactionRepositoryAsync.UpdateAsync(vouchertransaction);
                    return new Response<int>(vouchertransaction.Id);
                }
            }
        }
    }
}
