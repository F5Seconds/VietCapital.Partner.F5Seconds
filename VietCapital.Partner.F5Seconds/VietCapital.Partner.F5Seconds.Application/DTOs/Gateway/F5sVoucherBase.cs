using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VietCapital.Partner.F5Seconds.Application.DTOs.Gateway
{
    public class F5sVoucherBase
    {
        public int productId { get; set; }
        public string productNm { get; set; }
        public string productImg { get; set; }
        public float productPrice { get; set; }
        public int productTyp { get; set; } = 1;
        public int productSize { get; set; } = 0;
        public string productPartner { get; set; }
        public string brandNm { get; set; }
        public string brandLogo { get; set; }
    }

    public class F5sVoucherDetail : F5sVoucherBase
    {
        [Description("Mô tả sản phẩm")]
        public string productContent { get; set; }

        [Description("Điều khoản sử dụng")]
        public string productTerm { get; set; }

        [Description("Mảng chứa tất cả thông tin cửa hàng có thể sử dung voucher")]
        public List<F5sVoucherOffice> storeList { get; set; }
    }

    public class F5sVoucherOffice
    {
        public string storeNm { get; set; }
        public string storeAddr { get; set; }
        public string storePhone { get; set; }
        [Description("Số Lat trên bản đồ Google")]
        public float storeLat { get; set; }
        [Description("Số Long trên bản đồ Google")]
        public float storeLong { get; set; }
    }

    public class F5sBuyVoucher
    {
        public string propductId { get; set; }
        public int quantity { get; set; }
        public string transactionId { get; set; }
        public string customerId { get; set; }
        public string customerPhone { get; set; }
    }

    public class F5sVoucherCode
    {
        public string transactionId { get; set; }
        public string propductId { get; set; }
        public float productPrice { get; set; }
        public string customerPhone { get; set; }
        public string voucherCode { get; set; }
        public string expiryDate { get; set; }
    }

    public class BuyVoucherPayload
    {
        public string channel { get; set; } = "VIETCAPITAL";
        public string productCode { get; set; }
        public int quantity { get; set; } = 1;
        public string transactionId { get; set; } = Guid.NewGuid().ToString();
        public string customerId { get; set; }
        public string customerPhone { get; set; }
    }
}
