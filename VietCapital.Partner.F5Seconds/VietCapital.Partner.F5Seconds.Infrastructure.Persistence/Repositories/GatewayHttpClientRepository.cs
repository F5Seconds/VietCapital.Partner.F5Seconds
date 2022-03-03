using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
