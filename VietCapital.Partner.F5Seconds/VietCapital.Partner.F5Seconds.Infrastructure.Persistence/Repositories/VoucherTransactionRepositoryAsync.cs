using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter;
using VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetAllVoucherTransactions;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Contexts;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repository;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repositories
{
    public class VoucherTransactionRepositoryAsync : GenericRepositoryAsync<VoucherTransaction>, IVoucherTransactionRepositoryAsync
    {
        private readonly DbSet<VoucherTransaction> _voucherTransactions;
        public VoucherTransactionRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _voucherTransactions = dbContext.Set<VoucherTransaction>();
        }

        public async Task<PagedList<VoucherTransaction>> GetPagedVoucherTransByFilter(GetVoucherTransFilterParameter parameter)
        {
            var trans = _voucherTransactions
                .Include(p => p.Product)
                .Where(x => x.CustomerId.Equals(parameter.Cif.Trim()) && x.State.Equals(parameter.State)).AsQueryable();
            Search(ref trans,parameter.Search);
            return await PagedList<VoucherTransaction>.ToPagedList(trans.OrderByDescending(x => x.Id).AsNoTracking(), parameter.PageNumber, parameter.PageSize);
        }

        private void Search(ref IQueryable<VoucherTransaction> products, string search)
        {
            if (string.IsNullOrWhiteSpace(search)) return;
            search = $"%{search.Trim()}%";
            products = products.Where(x =>
                EF.Functions.Like(x.VoucherCode, search)
            );
        }

        public async Task<IReadOnlyList<VoucherTransaction>> GetVoucherTransactionByFilter(string cif, int state)
        {
            return await _voucherTransactions
                .Include(p => p.Product)
                .Where(x => x.CustomerId.Equals(cif.Trim()) && x.State.Equals(state))
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<bool> IsUniqueTransactionAsync(string transId)
        {
            return await _voucherTransactions.AnyAsync(x => x.TransactionId.Equals(transId.Trim()));
        }

        public async Task<VoucherTransaction> GetVoucherByTransId(string transId)
        {
            return await _voucherTransactions.SingleOrDefaultAsync(x=> x.TransactionId.Equals(transId.Trim()));
        }

        public async Task<PagedList<VoucherTransaction>> GetAllPagedListAsync(GetAllVoucherTransactionsParameter parameter)
        {
            var trans = _voucherTransactions
                .Include(p => p.Product).AsQueryable();
            Search(ref trans,parameter.Search);
            return await PagedList<VoucherTransaction>.ToPagedList(trans.OrderByDescending(x => x.Id).Where( p=> parameter.From != null ? p.Created.Date >= parameter.From.Value.Date :true).Where( p=> parameter.To != null ? p.Created.Date <= parameter.To.Value.Date :true).AsNoTracking(), parameter.PageNumber, parameter.PageSize);
        }

        public async Task<List<VoucherTransaction>> DoiSoatGiaoDichKhongKhopF5s(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            return await _voucherTransactions.FromSqlRaw($"CALL DoiSoatGiaoDichKhongKhopF5s('{ngayBatDau.ToString("yyyy-MM-dd")}','{ngayKetThuc.ToString("yyyy-MM-dd")}');").ToListAsync();
        }

        public async Task<List<VoucherTransaction>> DoiSoatGiaoDichKhongKhopBvb(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            return await _voucherTransactions.FromSqlRaw($"CALL DoiSoatGiaoDichKhongKhopBvb('{ngayBatDau.ToString("yyyy-MM-dd")}','{ngayKetThuc.ToString("yyyy-MM-dd")}');").ToListAsync();
        }

        public async Task<List<VoucherTransaction>> DoiSoatGiaoDichKhop(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            return await _voucherTransactions.FromSqlRaw($"CALL DoiSoatGiaoDichKhop('{ngayBatDau.ToString("yyyy-MM-dd")}','{ngayKetThuc.ToString("yyyy-MM-dd")}');").ToListAsync();
        }
    }
}
