using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
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
