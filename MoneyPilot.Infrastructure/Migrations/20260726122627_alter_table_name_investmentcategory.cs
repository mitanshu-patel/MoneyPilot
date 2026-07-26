using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alter_table_name_investmentcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investment_InvestmentCategory_CategoryId",
                table: "Investment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvestmentCategory",
                table: "InvestmentCategory");

            migrationBuilder.RenameTable(
                name: "InvestmentCategory",
                newName: "InvestmentCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvestmentCategories",
                table: "InvestmentCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Investment_InvestmentCategories_CategoryId",
                table: "Investment",
                column: "CategoryId",
                principalTable: "InvestmentCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investment_InvestmentCategories_CategoryId",
                table: "Investment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvestmentCategories",
                table: "InvestmentCategories");

            migrationBuilder.RenameTable(
                name: "InvestmentCategories",
                newName: "InvestmentCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvestmentCategory",
                table: "InvestmentCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Investment_InvestmentCategory_CategoryId",
                table: "Investment",
                column: "CategoryId",
                principalTable: "InvestmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
