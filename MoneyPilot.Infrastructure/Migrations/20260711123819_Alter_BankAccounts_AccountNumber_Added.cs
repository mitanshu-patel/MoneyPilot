using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_BankAccounts_AccountNumber_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountNumber",
                table: "BankAccounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "BankAccounts");
        }
    }
}
