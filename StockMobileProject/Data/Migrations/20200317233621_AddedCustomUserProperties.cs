using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockMobileProject.Data.Migrations
{
    public partial class AddedCustomUserProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cash",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Performance",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cash",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Performance",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "AspNetUsers");
        }
    }
}
