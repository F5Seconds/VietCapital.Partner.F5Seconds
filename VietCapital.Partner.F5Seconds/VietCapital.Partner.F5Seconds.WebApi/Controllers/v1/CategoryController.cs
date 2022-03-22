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
    }
}
