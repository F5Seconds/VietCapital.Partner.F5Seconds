using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities.Views;

namespace VietCapital.Partner.F5Seconds.Application.Features.GiaoDichTheoNgayViews.Queries.GetByStartEndDate
{
    public class GetByStartEndDateQuery : IRequest<Response<List<TrangThaiGiaoDichTheoNgayView>>>
    {
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public class GetByStartEndDateQueryHandler : IRequestHandler<GetByStartEndDateQuery, Response<List<TrangThaiGiaoDichTheoNgayView>>>
        {
            private readonly IGiaoDichTheoNgayViewRepositoryAsync _giaoDichTheoNgay;
            public GetByStartEndDateQueryHandler(IGiaoDichTheoNgayViewRepositoryAsync giaoDichTheoNgay)
            {
                _giaoDichTheoNgay = giaoDichTheoNgay;
            }
            public async Task<Response<List<TrangThaiGiaoDichTheoNgayView>>> Handle(GetByStartEndDateQuery request, CancellationToken cancellationToken)
            {
                var giaodich = await _giaoDichTheoNgay.TrangThaiGiaoDichTheoNgayViews(request.NgayBatDau, request.NgayKetThuc);
                return new Response<List<TrangThaiGiaoDichTheoNgayView>>(true,giaodich);
            }
        }
    }
}
