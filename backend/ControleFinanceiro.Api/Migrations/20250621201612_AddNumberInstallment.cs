using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFinanceiro.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberInstallment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numberInstallments",
                table: "CreditCardPurchases",
                newName: "NumberInstallments");

            migrationBuilder.AddColumn<int>(
                name: "NumberInstallment",
                table: "PurchaseInstallments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberInstallment",
                table: "PurchaseInstallments");

            migrationBuilder.RenameColumn(
                name: "NumberInstallments",
                table: "CreditCardPurchases",
                newName: "numberInstallments");
        }
    }
}
