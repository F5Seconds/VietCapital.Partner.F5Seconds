using MediatR;
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
        public async Task<IActionResult> GetListProduct()
        {
            return Ok(await Mediator.Send(new GetListProductQuery()));
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetailProduct(string code)
        {
            if(string.IsNullOrEmpty(code)) return BadRequest();
            return Ok(await Mediator.Send(new GetDetailProductQuery() { Code = code }));
        }
    }
}
