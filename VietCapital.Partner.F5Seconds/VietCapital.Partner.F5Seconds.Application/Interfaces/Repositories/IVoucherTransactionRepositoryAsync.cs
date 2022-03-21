using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter;
using VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetAllVoucherTransactions;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories
{
    public interface IVoucherTransactionRepositoryAsync : IGenericRepositoryAsync<VoucherTransaction>
    {
        Task<bool> IsUniqueTransactionAsync(string transId);
        Task<IReadOnlyList<VoucherTransaction>> GetVoucherTransactionByFilter(string cif, int state);
        Task<PagedList<VoucherTransaction>> GetPagedVoucherTransByFilter(GetVoucherTransFilterParameter parameter);
        Task<VoucherTransaction> GetVoucherByTransId(string transId);
        Task<PagedList<VoucherTransaction>> GetAllPagedListAsync(GetAllVoucherTransactionsParameter parameter);
        Task<List<VoucherTransaction>> DoiSoatGiaoDichKhongKhopF5s(DateTime ngayBatDau, DateTime ngayKetThuc);
        Task<List<VoucherTransaction>> DoiSoatGiaoDichKhongKhopBvb(DateTime ngayBatDau, DateTime ngayKetThuc);
        Task<List<VoucherTransaction>> DoiSoatGiaoDichKhop(DateTime ngayBatDau, DateTime ngayKetThuc);
        Task<string[]> GetAllTransaction();
        Task LoadTransactionToCache();
    }
}
