using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace VietCapital.Partner.F5Seconds.Domain.Entities.Views
{
    [Keyless]
    public class TrangThaiGiaoDichTheoNgayView
    {
        public virtual DateTime Ngay { get; set; }
        public int ChuaSuDung { get; set; }
        public int DaSuDung { get; set; }
        public int Huy { get; set; }
        public int HetHan { get; set; }
    }
}
