using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetAllVoucherTransactions;
using VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetVoucherTransactionById;

namespace VietCapital.Partner.F5Seconds.WebApp.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class TransactionController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Get([FromQuery] GetAllVoucherTransactionsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllVoucherTransactionsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber ,Search = filter.Search}));
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetVoucherTransactionByIdQuery { Id = id }));
        }
        // // PUT api/<controller>/5
        // [HttpPut("{id}")]
        // // [Authorize]
        // [AllowAnonymous]

        // public async Task<IActionResult> Put(int id, UpdateVoucherTransactionCommand command)
        // {
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(await Mediator.Send(command));
        // }
    }
}
