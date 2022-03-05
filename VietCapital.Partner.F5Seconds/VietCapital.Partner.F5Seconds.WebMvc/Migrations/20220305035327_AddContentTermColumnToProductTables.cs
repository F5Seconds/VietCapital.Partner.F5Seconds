using Microsoft.EntityFrameworkCore.Migrations;

namespace VietCapital.Partner.F5Seconds.WebMvc.Migrations
{
    public partial class AddContentTermColumnToProductTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Products",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Term",
                table: "Products",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Term",
                table: "Products");
        }
    }
}
