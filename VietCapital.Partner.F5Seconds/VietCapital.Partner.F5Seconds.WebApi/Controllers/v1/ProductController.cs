using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.DetailProduct;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.ListProduct;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VietCapital.Partner.F5Seconds.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/product")]
    public class ProductController : BaseApiController
    {
        [HttpGet("list")]
        public async Task<IActionResult> GetListProduct([FromQuery] GetListProductParameter filter)
        {
            return Ok(await Mediator.Send(new GetListProductQuery() { PageNumber = filter.PageNumber,PageSize = filter.PageSize,Search = filter.Search}));
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetailProduct([FromQuery] GetDetailProductParameter parameter)
        {
            return Ok(await Mediator.Send(new GetDetailProductQuery() { Code = parameter.Code }));
        }
    }
}
