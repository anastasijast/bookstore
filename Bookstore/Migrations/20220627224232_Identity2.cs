using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Migrations
{
    public partial class Identity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_userId",
                table: "User",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AspNetUsers_userId",
                table: "User",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AspNetUsers_userId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_userId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "User");
        }
    }
}
