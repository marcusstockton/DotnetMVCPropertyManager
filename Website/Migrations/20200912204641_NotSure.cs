using Microsoft.EntityFrameworkCore.Migrations;

namespace Website.Migrations
{
    public partial class NotSure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Expires",
                table: "DocumentTypes",
                nullable: false,
                defaultValue: false);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTypes_AspNetUsers_OwnerId",
                table: "DocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTypes_OwnerId",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "DocumentTypes");
        }
    }
}
