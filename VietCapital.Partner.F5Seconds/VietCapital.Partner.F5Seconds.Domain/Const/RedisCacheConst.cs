using Microsoft.Extensions.Caching.Distributed;
using System;

namespace VietCapital.Partner.F5Seconds.Domain.Const
{
    public static class RedisCacheConst
    {
        public static string ProductKey { get; set; } = "PRODUCTS_CACHE";
        public static string CategoryKey { get; set; } = "CATEGORIES_CACHE";
        public static int SetAbsoluteExpiration { get; set; } = 10;
        public static int SetSlidingExpiration { get; set; } = 2;
        public static DistributedCacheEntryOptions CacheEntryOptions { get; set; } = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(SetAbsoluteExpiration))
            .SetSlidingExpiration(TimeSpan.FromMinutes(SetSlidingExpiration));
    }
}
