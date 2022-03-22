using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Domain.Const;

namespace VietCapital.Partner.F5Seconds.Application.Features.Transactions.Commands.CreateVoucherTransactionCommand
{
    public class CreateVoucherTransactionCommandValidator : AbstractValidator<CreateVoucherTransactionCommand> 
    {
        private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IDistributedCache _distributedCache;
        private string serializedTransList;
        private string[] transList;
        private string serializedProductCodeList;
        private string[] productCodeList;
        private readonly IRedisClient _redisClient;
        private readonly IDatabase _redisDatabase;
        private readonly ILogger<CreateVoucherTransactionCommandValidator> _logger;
        public CreateVoucherTransactionCommandValidator(
            IVoucherTransactionRepositoryAsync voucherTransaction,
            IProductRepositoryAsync productRepository,
            IDistributedCache distributedCache,
            IDatabase redisDatabase,
            IRedisClient redisClient,
            ILogger<CreateVoucherTransactionCommandValidator> logger)
        {
            _voucherTransaction = voucherTransaction;
            _productRepository = productRepository;
            _distributedCache = distributedCache;
            _logger = logger;
            _redisClient = redisClient;
            _redisDatabase = redisDatabase;
            RuleFor(p => p.transactionId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniqueTransId).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.productCode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsExitedProduct).WithMessage("{PropertyName} not exists.");

            RuleFor(p => p.customerId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.quantity)
                .NotNull()
                .GreaterThan(0);
        }

        private async Task<bool> IsUniqueTransId(string transId, CancellationToken cancellationToken)
        {
            var redisTransList = await _distributedCache.GetAsync(RedisCacheConst.TransactionKey);
            if (redisTransList != null)
            {
                serializedTransList = Encoding.UTF8.GetString(redisTransList);
                transList = JsonConvert.DeserializeObject<string[]>(serializedTransList);
                return !transList.Contains(transId);
            }
            await _voucherTransaction.LoadTransactionToCache();
            var result = await _voucherTransaction.IsUniqueTransactionAsync(transId);
            return !result;
        }

        private async Task<bool> IsExitedProduct(string productCode, CancellationToken cancellationToken)
        {
            var redisProductCodeList = await _distributedCache.GetAsync(RedisCacheConst.ProductCodeKey);
            if(redisProductCodeList != null)
            {
                serializedProductCodeList = Encoding.UTF8.GetString(redisProductCodeList);
                productCodeList = JsonConvert.DeserializeObject<string[]>(serializedProductCodeList);
                return productCodeList.Contains(productCode);
            }
            await _productRepository.LoadProductCodeToCache();
            return await _productRepository.IsExitedByCode(productCode);
        }
    }
}
