using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces
{
    public interface IGatewayHttpClientService
    {
        Task<Response<List<Product>>> ListProduct();
        Task<Response<F5sVoucherDetail>> DetailProduct(string code);
        Task<Response<List<F5sVoucherCode>>> BuyProduct(BuyVoucherPayload code);
    }
}
