using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.GiaoDichTheoNgayViews.Queries.GetByStartEndDate;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;

namespace VietCapital.Partner.F5Seconds.WebApp.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BieuDoController : BaseApiController
    {
        [HttpGet("giaodich-theongay")]
        public async Task<IActionResult> GetGiaoDichTheoNgay([FromQuery] GetByStartEndDateParameter query)
        {
            return Ok(await Mediator.Send(new GetByStartEndDateQuery() { NgayBatDau = query.NgayBatDau,NgayKetThuc = query.NgayKetThuc}));
        }
    }
}
