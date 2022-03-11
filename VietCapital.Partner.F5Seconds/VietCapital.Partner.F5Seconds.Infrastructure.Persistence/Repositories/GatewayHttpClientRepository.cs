using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Application.Parameters;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repositories
{
    public class GatewayHttpClientRepository : IGatewayHttpClientService
    {
        private readonly HttpClient _client;
        public GatewayHttpClientRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<Response<List<F5sVoucherCode>>> BuyProduct(BuyVoucherPayload payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(GatewayUriParamater.BuyProduct, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResponseBase>(jsonString);
                if (result.Succeeded) return JsonConvert.DeserializeObject<Response<List<F5sVoucherCode>>>(jsonString);
                return new Response<List<F5sVoucherCode>>(false, null, result.Message, result.Errors);
            }
            return new Response<List<F5sVoucherCode>>(false,null,response.ReasonPhrase);
        }

        public async Task<Response<F5sVoucherDetail>> DetailProduct(string code)
        {
            var response = await _client.GetAsync($"{GatewayUriParamater.DetailProduct}/{code}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Response<F5sVoucherDetail>>(jsonString);
            }
            return new Response<F5sVoucherDetail>(false, null, response.ReasonPhrase);
        }

        public async Task<Response<List<Product>>> ListProduct()
        {
            var response = await _client.GetAsync($"{GatewayUriParamater.ListProduct}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Response<List<Product>>>(jsonString);
            }
            return new Response<List<Product>>(false,null,response.ReasonPhrase);
        }
    }
}
