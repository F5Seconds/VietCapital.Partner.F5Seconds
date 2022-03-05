using Microsoft.EntityFrameworkCore.Migrations;

namespace VietCapital.Partner.F5Seconds.WebMvc.Migrations
{
    public partial class AddPointThumnailColumnToProductTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Point",
                table: "Products",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Products",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Point",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Products");
        }
    }
}
