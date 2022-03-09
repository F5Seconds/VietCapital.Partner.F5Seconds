using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using System.Reflection;

namespace VietCapital.Partner.F5Seconds.WebMvc.Migrations
{
    public partial class ProductView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = typeof(ProductView).Namespace + "./Migrations/Scripts/CreateProductView.sql";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string sqlResult = reader.ReadToEnd();
                    migrationBuilder.Sql(sqlResult);
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
