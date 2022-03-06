using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.DetailCategory;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.ListCategory;

namespace VietCapital.Partner.F5Seconds.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/category")]
    public class CategoryController : BaseApiController
    {
        [HttpGet("list")]
        public async Task<IActionResult> GetListCategory()
        {
            return Ok(await Mediator.Send(new GetListCategoryQuery()));
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetailCategory(int? id)
        {
            if(id is null) return BadRequest();
            return Ok(await Mediator.Send(new GetDetailCategoryQuery() { Id = id}));
        }
    }
}
