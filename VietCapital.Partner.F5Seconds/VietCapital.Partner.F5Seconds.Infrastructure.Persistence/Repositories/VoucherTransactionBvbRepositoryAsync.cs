using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Domain.Entities;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Contexts;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repository;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repositories
{
    public class VoucherTransactionBvbRepositoryAsync : GenericRepositoryAsync<VoucherTransactionsBvb>, IVoucherTransactionBvbRepositoryAsync
    {
        private readonly DbSet<VoucherTransactionsBvb> _voucherTransactionBvbs;
        private readonly ApplicationDbContext _context;
        public VoucherTransactionBvbRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _voucherTransactionBvbs = dbContext.Set<VoucherTransactionsBvb>();
            _context = dbContext;
        }

        public async Task TruncateTable()
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE VoucherTransactionsBvb");
        }
    }
}
