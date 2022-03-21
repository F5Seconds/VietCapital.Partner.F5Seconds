using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Domain.Const;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.WebApi.Consumer
{
    public class VoucherTransactionConsumer : IConsumer<VoucherTransaction>
    {
        private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
        private readonly IDistributedCache _distributedCache;
        private string serializedTransList;
        private string[] transList;
        private byte[] redisTransList;
        public VoucherTransactionConsumer(
            IVoucherTransactionRepositoryAsync voucherTransaction,
            IDistributedCache distributedCache)
        {
            _voucherTransaction = voucherTransaction;
            _distributedCache = distributedCache;
        }
        public async Task Consume(ConsumeContext<VoucherTransaction> context)
        {
            await _voucherTransaction.AddAsync(context.Message);
        }

        public async Task UpdateTransCache(string transId)
        {
            redisTransList = await _distributedCache.GetAsync(RedisCacheConst.TransactionKey);
            if (redisTransList != null)
            {
                serializedTransList = Encoding.UTF8.GetString(redisTransList);
                transList = JsonConvert.DeserializeObject<string[]>(serializedTransList);
                if (!transList.Contains(transId))
                {
                    transList.Append(transId);
                    serializedTransList = JsonConvert.SerializeObject(transList);
                    redisTransList = Encoding.UTF8.GetBytes(serializedTransList);
                    await _distributedCache.SetAsync(RedisCacheConst.TransactionKey, redisTransList, RedisCacheConst.CacheEntryOptions);
                }
            }
        }
    }
}
