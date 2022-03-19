using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VietCapital.Partner.F5Seconds.Application.Filters;

namespace VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter
{
    public class GetVoucherTransFilterParameter : RequestParameter
    {
        [Required]
        public string Cif { get; set; }
        [Required]
        public int State { get; set; }
    }
}
