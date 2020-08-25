using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Website.Migrations
{
    public partial class PropertyDocFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDocuments_DocumentTypes_DocumentTypeId",
                table: "PropertyDocuments");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentTypeId",
                table: "PropertyDocuments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDocuments_DocumentTypes_DocumentTypeId",
                table: "PropertyDocuments",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDocuments_DocumentTypes_DocumentTypeId",
                table: "PropertyDocuments");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentTypeId",
                table: "PropertyDocuments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDocuments_DocumentTypes_DocumentTypeId",
                table: "PropertyDocuments",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
