using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.ShoppingCartApi.Migrations
{
    public partial class ActualDiscountAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ActualDiscountAmount",
                table: "CartHeaders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDiscountAmount",
                table: "CartHeaders");
        }
    }
}
