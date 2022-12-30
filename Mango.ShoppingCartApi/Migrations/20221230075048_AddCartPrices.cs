using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.ShoppingCartApi.Migrations
{
    public partial class AddCartPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountAmount",
                table: "CartHeaders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FinalPrice",
                table: "CartHeaders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "CartHeaders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "CartHeaders");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "CartHeaders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "CartHeaders");
        }
    }
}
