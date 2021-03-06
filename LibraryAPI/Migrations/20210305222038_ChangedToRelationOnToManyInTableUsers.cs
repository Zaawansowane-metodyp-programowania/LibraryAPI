using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryAPI.Migrations
{
    public partial class ChangedToRelationOnToManyInTableUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_UsersId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Books_UsersId",
                table: "Books",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_UsersId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UsersId",
                table: "Books",
                column: "UsersId",
                unique: true,
                filter: "[UsersId] IS NOT NULL");
        }
    }
}
