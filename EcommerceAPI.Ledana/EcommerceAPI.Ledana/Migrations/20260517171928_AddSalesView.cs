using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceAPI.Ledana.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
    CREATE VIEW v_SalesWithTotal AS
    SELECT s.Id,
           s.Date,
           COALESCE(SUM(sp.TotalPrice), 0) AS TotalPrice
    FROM Sales s
    LEFT JOIN SaleProduct sp ON sp.SalesId = s.Id
    GROUP BY s.Id, s.Date;
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW v_SalesWithTotal;");

        }
    }
}
