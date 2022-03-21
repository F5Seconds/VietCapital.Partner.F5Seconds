using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Domain.Entities.Views;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories
{
    public interface IGiaoDichTheoNgayViewRepositoryAsync : IGenericRepositoryAsync<TrangThaiGiaoDichTheoNgayView>
    {
        Task<List<TrangThaiGiaoDichTheoNgayView>> TrangThaiGiaoDichTheoNgayViews(DateTime ngayBatDau,DateTime ngayKetThuc);
    }
}