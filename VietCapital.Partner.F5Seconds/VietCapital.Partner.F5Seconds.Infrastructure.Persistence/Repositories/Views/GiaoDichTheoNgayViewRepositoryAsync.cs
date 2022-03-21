using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Domain.Entities.Views;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Contexts;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repository;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repositories.Views
{
    public class GiaoDichTheoNgayViewRepositoryAsync : GenericRepositoryAsync<TrangThaiGiaoDichTheoNgayView>, IGiaoDichTheoNgayViewRepositoryAsync
    {
        private readonly DbSet<TrangThaiGiaoDichTheoNgayView> _giaoDichTheoNgayViews;
        public GiaoDichTheoNgayViewRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _giaoDichTheoNgayViews = dbContext.Set<TrangThaiGiaoDichTheoNgayView>();
        }

        public async Task<List<TrangThaiGiaoDichTheoNgayView>> TrangThaiGiaoDichTheoNgayViews(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            return await _giaoDichTheoNgayViews.Where(x => x.Ngay >= ngayBatDau && x.Ngay <= ngayKetThuc).ToListAsync();    
        }
    }
}
