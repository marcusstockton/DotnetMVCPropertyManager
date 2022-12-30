using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Data.Migrations
{
    /// <inheritdoc />
    public partial class Nationalitiesadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Tenants");

            migrationBuilder.AddColumn<int>(
                name: "NationalityId",
                table: "Tenants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_NationalityId",
                table: "Tenants",
                column: "NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Nationalities_NationalityId",
                table: "Tenants",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Nationalities_NationalityId",
                table: "Tenants");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_NationalityId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                table: "Tenants");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
