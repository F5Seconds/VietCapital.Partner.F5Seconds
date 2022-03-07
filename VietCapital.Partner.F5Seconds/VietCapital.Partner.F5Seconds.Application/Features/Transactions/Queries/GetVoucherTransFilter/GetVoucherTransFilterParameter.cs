using VietCapital.Partner.F5Seconds.Application.Filters;

namespace VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter
{
    public class GetVoucherTransFilterParameter : RequestParameter
    {
        public string Cif { get; set; }
        public int State { get; set; }
    }
}
