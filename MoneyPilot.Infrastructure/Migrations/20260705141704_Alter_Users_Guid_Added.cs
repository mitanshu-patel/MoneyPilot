using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Users_Guid_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserOId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: null);

            migrationBuilder.Sql("UPDATE Users SET UserOId = NEWID() WHERE UserOId IS NULL");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserOId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserOId",
                table: "Users");
        }
    }
}
