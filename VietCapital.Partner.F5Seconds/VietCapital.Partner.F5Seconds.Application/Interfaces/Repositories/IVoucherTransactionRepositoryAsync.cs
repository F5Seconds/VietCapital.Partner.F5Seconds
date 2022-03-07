using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories
{
    public interface IVoucherTransactionRepositoryAsync : IGenericRepositoryAsync<VoucherTransaction>
    {
        Task<bool> IsUniqueTransactionAsync(string transId);
        Task<IReadOnlyList<VoucherTransaction>> GetVoucherTransactionByFilter(string cif, int state);
        Task<PagedList<VoucherTransaction>> GetPagedVoucherTransByFilter(GetVoucherTransFilterParameter parameter);
    }
}
