using System;
using System.Collections.Generic;
using System.Text;

namespace VietCapital.Partner.F5Seconds.Application.Parameters
{
    public class GatewayUriParamater
    {
        public static string ListProduct { get; set; } = "/api/gateway/vouchers";
        public static string DetailProduct { get; set; } = "/api/gateway/voucher";
        public static string BuyProduct { get; set; } = "/api/gateway/transaction";
    }
}
