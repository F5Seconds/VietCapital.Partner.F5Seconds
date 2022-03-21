using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.DetailCategory;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.ListCategory;

namespace VietCapital.Partner.F5Seconds.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/category")]
    public class CategoryController : BaseApiController
    {
        private readonly IDistributedCache _distributedCache;
        public CategoryController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("list")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = true)]
        public async Task<IActionResult> GetListCategory([FromQuery] GetListCategoryParameter filter)
        {
            return Ok(await Mediator.Send(new GetListCategoryQuery() { PageNumber = filter.PageNumber,PageSize = filter.PageSize,Search = filter.Search}));
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetailCategory([FromQuery] GetDetailCategoryParameter parameter)
        {
            return Ok(await Mediator.Send(new GetDetailCategoryQuery() { Id = parameter.Id }));
        }

        [HttpGet("the-time")]
        public string GetTheTime()
        {
            var cacheKey = "TheTime";
            var currentTime = DateTime.Now.ToString();
            var cachedTime = _distributedCache.GetString(cacheKey);
            if (string.IsNullOrEmpty(cachedTime))
            {
                // cachedTime = "Expired";
                // Cache expire trong 5s
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(5));
                // Nạp lại giá trị mới cho cache
                _distributedCache.SetString(cacheKey, currentTime, options);
                cachedTime = _distributedCache.GetString(cacheKey);
            }
            var result = $"Current Time : {currentTime} \nCached  Time : {cachedTime}";
            return result;
        }

        [HttpGet("request-caching")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ContentResult GetTimeMS() => Content(
                  DateTime.Now.Millisecond.ToString());
    }
}
