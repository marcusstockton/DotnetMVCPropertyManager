using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Website.Data.Migrations
{
    public partial class DocumentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropColumn(
            //     name: "DocumentType",
            //     table: "PropertyDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "PropertyDocuments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDocuments_DocumentTypeId",
                table: "PropertyDocuments",
                column: "DocumentTypeId");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_PropertyDocuments_DocumentTypes_DocumentTypeId",
            //     table: "PropertyDocuments",
            //     column: "DocumentTypeId",
            //     principalTable: "DocumentTypes",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_PropertyDocuments_DocumentTypes_DocumentTypeId",
            //     table: "PropertyDocuments");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_PropertyDocuments_DocumentTypeId",
                table: "PropertyDocuments");

            // migrationBuilder.DropColumn(
            //     name: "DocumentTypeId",
            //     table: "PropertyDocuments");

            migrationBuilder.AddColumn<int>(
                name: "DocumentType",
                table: "PropertyDocuments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}