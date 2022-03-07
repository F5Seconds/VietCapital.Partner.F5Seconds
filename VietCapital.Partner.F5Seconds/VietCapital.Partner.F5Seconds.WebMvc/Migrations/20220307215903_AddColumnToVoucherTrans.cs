using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VietCapital.Partner.F5Seconds.WebMvc.Migrations
{
    public partial class AddColumnToVoucherTrans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsedBrand",
                table: "VoucherTransactions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedTime",
                table: "VoucherTransactions",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsedBrand",
                table: "VoucherTransactions");

            migrationBuilder.DropColumn(
                name: "UsedTime",
                table: "VoucherTransactions");
        }
    }
}
