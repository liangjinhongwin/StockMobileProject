using Microsoft.EntityFrameworkCore.Migrations;

namespace StockMobileProject.Data.Migrations
{
    public partial class AddedCash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cash",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cash",
                table: "AspNetUsers");
        }
    }
}
