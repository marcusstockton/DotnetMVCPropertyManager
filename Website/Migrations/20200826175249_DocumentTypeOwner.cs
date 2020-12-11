using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Website.Migrations
{
    public partial class DocumentTypeOwner : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "DocumentTypes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_OwnerId",
                table: "DocumentTypes",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTypes_AspNetUsers_OwnerId",
                table: "DocumentTypes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_DocumentTypes_AspNetUsers_OwnerId",
                table: "DocumentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDocuments_DocumentTypes_DocumentTypeId",
                table: "PropertyDocuments");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTypes_OwnerId",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "DocumentTypes");

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