using System;
using System.Collections.Generic;
using System.Text;

namespace VietCapital.Partner.F5Seconds.Domain.Const
{
    public static class ResponseConst
    {
        public static string NotData { get; set; } = "Không có dữ liệu";
        public static string PartnerNotData { get; set; } = "Đối tác trả về dữ liệu rỗng";
        public static string UnknowError { get; set; } = "Unknown Error Occurred (Lỗi khác)";
    }
}
