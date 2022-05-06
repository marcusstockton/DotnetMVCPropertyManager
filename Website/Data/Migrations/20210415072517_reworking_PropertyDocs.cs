using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Website.Data.Migrations
{
    public partial class reworking_PropertyDocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expires",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "DocumentTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActiveFrom",
                table: "PropertyDocuments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Expires",
                table: "PropertyDocuments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveFrom",
                table: "PropertyDocuments");

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "PropertyDocuments");

            migrationBuilder.AddColumn<bool>(
                name: "Expires",
                table: "DocumentTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "DocumentTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}