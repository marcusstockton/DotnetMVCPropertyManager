using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addingexpirydetailstodocumenttype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Expires",
                table: "DocumentTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiryDate",
                table: "DocumentTypes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expires",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "DocumentTypes");
        }
    }
}
