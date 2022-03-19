using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories
{
    public interface IVoucherTransactionBvbRepositoryAsync : IGenericRepositoryAsync<VoucherTransactionsBvb>
    {
        Task TruncateTable();
    }
}
