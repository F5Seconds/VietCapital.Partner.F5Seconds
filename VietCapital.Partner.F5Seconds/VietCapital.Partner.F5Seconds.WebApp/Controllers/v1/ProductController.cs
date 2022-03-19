using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Commands.CreateProduct;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Commands.DeleteProductByIdCommand;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Commands.UpdateProductCommand;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.GetAllProducts;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.GetProductById;

namespace VietCapital.Partner.F5Seconds.WebApp.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class ProductController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Get([FromQuery] GetAllProductsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllProductsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber ,Search = filter.Search}));
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }
        // POST api/<controller>
        [HttpPost]
        // [Authorize]
        [AllowAnonymous]

        public async Task<IActionResult> Post(CreateProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        // [Authorize]
        [AllowAnonymous]

        public async Task<IActionResult> Put(int id, UpdateProductCommand command)
        {
           if (id != command.Id)
           {
               return BadRequest();
           }
           return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        // [Authorize]
        [AllowAnonymous]

        public async Task<IActionResult> Delete(int id)
        {
           return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        }
    }
}
