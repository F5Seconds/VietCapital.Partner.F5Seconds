using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Commands.CreateCategory;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Commands.DeleteCategoryByIdCommand;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Commands.UpdateCategoryCommand;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.GetAllCategories;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.GetCategoryById;

namespace VietCapital.Partner.F5Seconds.WebApp.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class CategoryController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Get([FromQuery] GetAllCategoriesParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllCategoriesQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber ,Search = filter.Search}));
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetCategoryByIdQuery { Id = id }));
        }
        // POST api/<controller>
        [HttpPost]
        // [Authorize]
        [AllowAnonymous]

        public async Task<IActionResult> Post(CreateCategoriesCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        //[HttpPost("list")]
        //[Authorize]
        //public async Task<IActionResult> PostList(CreateProductCommand command)
        //{
        //    return Ok(await Mediator.Send(command));
        //}
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        // [Authorize]
        [AllowAnonymous]

        public async Task<IActionResult> Put(int id, UpdateCategoryCommand command)
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
           return Ok(await Mediator.Send(new DeleteCategoryByIdCommand { Id = id }));
        }
    }
}
