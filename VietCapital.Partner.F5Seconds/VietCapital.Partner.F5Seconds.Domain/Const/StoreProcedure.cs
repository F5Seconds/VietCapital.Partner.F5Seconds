namespace VietCapital.Partner.F5Seconds.Domain.Const
{
    public static class StoreProcedure
    {
        public static string DoiSoatGiaoDichKhongKhopF5s { get; set; } = @"
        DROP PROCEDURE IF EXISTS DoiSoatGiaoDichKhongKhopF5s;
		CREATE PROCEDURE DoiSoatGiaoDichKhongKhopF5s(
			IN NgayBatDau VARCHAR(255),
			IN NgayKetThuc VARCHAR(255)
		)
		BEGIN
			SELECT * FROM VoucherTransactions where TransactionId NOT IN(
				SELECT TransactionId
				FROM
				 (
					 SELECT t1.ProductId, t1.TransactionId, t1.State, t1.VoucherCode
					 FROM VoucherTransactions as t1 
					 WHERE t1.Created between NgayBatDau and NgayKetThuc
					 UNION ALL
					 SELECT t2.ProductId, t2.TransactionId, t2.State, t2.VoucherCode
					 FROM VoucherTransactionsBvb as t2
				)  t
				GROUP BY ProductId, TransactionId, State, VoucherCode
				HAVING COUNT(*) > 1
				ORDER BY ProductId
			);
		END;";

		public static string DoiSoatGiaoDichKhongKhopBvb { get; set; } = @"
		DROP PROCEDURE IF EXISTS DoiSoatGiaoDichKhongKhopBvb;
		CREATE PROCEDURE DoiSoatGiaoDichKhongKhopBvb(
			IN NgayBatDau VARCHAR(255),
			IN NgayKetThuc VARCHAR(255)
		)
		BEGIN
			SELECT * FROM VoucherTransactionsBvb where TransactionId NOT IN(
			SELECT TransactionId
				FROM
				 (
					 SELECT t1.ProductId, t1.TransactionId, t1.State, t1.VoucherCode
					 FROM VoucherTransactions as t1
					 UNION ALL
					 SELECT t2.ProductId, t2.TransactionId, t2.State, t2.VoucherCode
					 FROM VoucherTransactionsBvb as t2
				)  t
				GROUP BY ProductId, TransactionId, State, VoucherCode
				HAVING COUNT(*) > 1
				ORDER BY ProductId
			);
		END;";

		public static string DoiSoatGiaoDichKhop { get; set; } = @"
		DROP PROCEDURE IF EXISTS DoiSoatGiaoDichKhop;
		CREATE PROCEDURE DoiSoatGiaoDichKhop(
			IN NgayBatDau VARCHAR(255),
			IN NgayKetThuc VARCHAR(255)
		)
		BEGIN
			SELECT * FROM VoucherTransactionsBvb where TransactionId NOT IN(
			SELECT TransactionId
				FROM
				 (
					 SELECT t1.ProductId, t1.TransactionId, t1.State, t1.VoucherCode
					 FROM VoucherTransactions as t1
					 UNION ALL
					 SELECT t2.ProductId, t2.TransactionId, t2.State, t2.VoucherCode
					 FROM VoucherTransactionsBvb as t2
				)  t
				GROUP BY ProductId, TransactionId, State, VoucherCode
				HAVING COUNT(*) = 1
				ORDER BY ProductId
			);
		END;";
    }
}
