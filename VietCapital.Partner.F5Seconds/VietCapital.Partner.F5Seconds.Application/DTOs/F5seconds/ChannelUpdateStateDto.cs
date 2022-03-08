using System;

namespace VietCapital.Partner.F5Seconds.Application.DTOs.F5seconds
{
    public class ChannelUpdateStateDto
    {
        public string TransactionId { get; set; }
        public string ProductCode { get; set; }
        public string Channel { get; set; }
        public int State { get; set; }
        public string UsedBrand { get; set; }
        public DateTime? UsedTime { get; set; }
    }
}
