using Microsoft.EntityFrameworkCore.Migrations;

namespace StockMobileProject.Data.Migrations
{
    public partial class AddedUserStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStocks",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    Symbol = table.Column<string>(nullable: false),
                    IsWatched = table.Column<bool>(nullable: false),
                    PurchasedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStocks", x => new { x.Email, x.Symbol });
                    table.ForeignKey(
                        name: "FK_UserStocks_AspNetUsers_Email",
                        column: x => x.Email,
                        principalTable: "AspNetUsers",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStocks_Email",
                table: "UserStocks",
                column: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStocks");
        }
    }
}
