using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryAPI.Migrations
{
    public partial class ChangedUsersIdToNullableInTableBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UsersId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UsersId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "UsersId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Books_UsersId",
                table: "Books",
                column: "UsersId",
                unique: true,
                filter: "[UsersId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UsersId",
                table: "Books",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UsersId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UsersId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "UsersId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UsersId",
                table: "Books",
                column: "UsersId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UsersId",
                table: "Books",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
