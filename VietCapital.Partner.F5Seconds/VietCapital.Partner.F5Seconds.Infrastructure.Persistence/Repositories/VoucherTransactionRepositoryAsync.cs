using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter;
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
    }
}
