using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Website.Migrations
{
    public partial class NotSure3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpriyDate",
                table: "DocumentTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "DocumentTypes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "DocumentTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpriyDate",
                table: "DocumentTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}