using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFinanceiro.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurringAccountReceivable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecurringAccountReceivableId",
                table: "AccountsReceivable",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecurringAccountsReceivable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    Period = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringAccountsReceivable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringAccountsReceivable_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecurringAccountsReceivable_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_RecurringAccountReceivableId",
                table: "AccountsReceivable",
                column: "RecurringAccountReceivableId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringAccountsReceivable_CategoryId",
                table: "RecurringAccountsReceivable",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringAccountsReceivable_UserId",
                table: "RecurringAccountsReceivable",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsReceivable_RecurringAccountsReceivable_RecurringAcco~",
                table: "AccountsReceivable",
                column: "RecurringAccountReceivableId",
                principalTable: "RecurringAccountsReceivable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsReceivable_RecurringAccountsReceivable_RecurringAcco~",
                table: "AccountsReceivable");

            migrationBuilder.DropTable(
                name: "RecurringAccountsReceivable");

            migrationBuilder.DropIndex(
                name: "IX_AccountsReceivable_RecurringAccountReceivableId",
                table: "AccountsReceivable");

            migrationBuilder.DropColumn(
                name: "RecurringAccountReceivableId",
                table: "AccountsReceivable");
        }
    }
}
