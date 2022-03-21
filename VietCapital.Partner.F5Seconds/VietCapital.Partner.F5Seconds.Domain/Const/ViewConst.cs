using System;
using System.Collections.Generic;
using System.Text;

namespace VietCapital.Partner.F5Seconds.Domain.Const
{
    public static class ViewConst
    {
        public static string TrangThaiGiaoDichTheoNgay { get; set; } = @"
        CREATE or REPLACE VIEW TrangThaiGiaoDichTheoNgayViews
        AS
        SELECT Ngay,
			         SUM(CASE WHEN TrangThai = '1' THEN SoLuong ELSE 0 end) as 'ChuaSuDung',
			         SUM(CASE WHEN TrangThai = '2' THEN SoLuong ELSE 0 END) AS 'DaSuDung',
			         SUM(CASE WHEN TrangThai = '3' THEN SoLuong ELSE 0 END) AS 'Huy',
			         SUM(CASE WHEN TrangThai = '4' THEN SoLuong ELSE 0 END) AS 'HetHan'
        FROM(
        (SELECT DATE(Created) as Ngay, COUNT(ProductId) as SoLuong,'1' as TrangThai
        FROM VoucherTransactions
        WHERE State = 1
        GROUP BY DATE(Created))
        UNION ALL 
        (SELECT DATE(Created) as Ngay, COUNT(ProductId) as SoLuong,'2' as TrangThai
        FROM VoucherTransactions
        WHERE State = 2
        GROUP BY DATE(Created))
        UNION ALL
        (SELECT DATE(Created) as Ngay, COUNT(ProductId) as SoLuong,'3' as TrangThai
        FROM VoucherTransactions
        WHERE State = 3
        GROUP BY DATE(Created))
        UNION ALL
        (SELECT DATE(Created) as Ngay, COUNT(ProductId) as SoLuong,'4' as TrangThai
        FROM VoucherTransactions
        WHERE State = 4
        GROUP BY DATE(Created))) AS t
        GROUP BY Ngay ";
    }
}
