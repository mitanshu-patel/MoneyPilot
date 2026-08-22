using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alter_category_transaction_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investment_InvestmentCategories_CategoryId",
                table: "Investment");

            migrationBuilder.DropForeignKey(
                name: "FK_Investment_Transaction_TransactionId",
                table: "Investment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Investment",
                table: "Investment");

            migrationBuilder.DropColumn(
                name: "AutoDebitDate",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Investment",
                newName: "Investments");

            migrationBuilder.RenameIndex(
                name: "IX_Investment_TransactionId",
                table: "Investments",
                newName: "IX_Investments_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_Investment_CategoryId",
                table: "Investments",
                newName: "IX_Investments_CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "AutoDebitDay",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAutoPayment",
                table: "InvestmentCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasAutoPayment",
                table: "ExpenseCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Investments",
                table: "Investments",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "ExpenseCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "HasAutoPayment",
                value: false);

            migrationBuilder.UpdateData(
                table: "ExpenseCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "HasAutoPayment",
                value: false);

            migrationBuilder.UpdateData(
                table: "ExpenseCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "HasAutoPayment" },
                values: new object[] { "Subscriptions", true });

            migrationBuilder.UpdateData(
                table: "ExpenseCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "HasAutoPayment",
                value: false);

            migrationBuilder.UpdateData(
                table: "ExpenseCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "HasAutoPayment",
                value: true);

            migrationBuilder.UpdateData(
                table: "ExpenseCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "HasAutoPayment",
                value: false);

            migrationBuilder.UpdateData(
                table: "InvestmentCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "HasAutoPayment",
                value: false);

            migrationBuilder.UpdateData(
                table: "InvestmentCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "HasAutoPayment",
                value: true);

            migrationBuilder.UpdateData(
                table: "InvestmentCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "HasAutoPayment",
                value: false);

            migrationBuilder.UpdateData(
                table: "InvestmentCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "HasAutoPayment",
                value: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_InvestmentCategories_CategoryId",
                table: "Investments",
                column: "CategoryId",
                principalTable: "InvestmentCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_Transaction_TransactionId",
                table: "Investments",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_InvestmentCategories_CategoryId",
                table: "Investments");

            migrationBuilder.DropForeignKey(
                name: "FK_Investments_Transaction_TransactionId",
                table: "Investments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Investments",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "AutoDebitDay",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "HasAutoPayment",
                table: "InvestmentCategories");

            migrationBuilder.DropColumn(
                name: "HasAutoPayment",
                table: "ExpenseCategories");

            migrationBuilder.RenameTable(
                name: "Investments",
                newName: "Investment");

            migrationBuilder.RenameIndex(
                name: "IX_Investments_TransactionId",
                table: "Investment",
                newName: "IX_Investment_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_Investments_CategoryId",
                table: "Investment",
                newName: "IX_Investment_CategoryId");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AutoDebitDate",
                table: "Transaction",
                type: "date",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Investment",
                table: "Investment",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "ExpenseCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Category",
                value: "Entertainment");

            migrationBuilder.AddForeignKey(
                name: "FK_Investment_InvestmentCategories_CategoryId",
                table: "Investment",
                column: "CategoryId",
                principalTable: "InvestmentCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Investment_Transaction_TransactionId",
                table: "Investment",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
