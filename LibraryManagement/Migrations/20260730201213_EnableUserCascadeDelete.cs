using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class EnableUserCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequests_Users_User_id",
                table: "BorrowRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Users_User_id",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuedBooks_BorrowRequests_borrowRequestid",
                table: "IssuedBooks");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequests_Users_User_id",
                table: "BorrowRequests",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Users_User_id",
                table: "CartItems",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedBooks_BorrowRequests_borrowRequestid",
                table: "IssuedBooks",
                column: "borrowRequestid",
                principalTable: "BorrowRequests",
                principalColumn: "borrowRequestid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequests_Users_User_id",
                table: "BorrowRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Users_User_id",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuedBooks_BorrowRequests_borrowRequestid",
                table: "IssuedBooks");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequests_Users_User_id",
                table: "BorrowRequests",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Users_User_id",
                table: "CartItems",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedBooks_BorrowRequests_borrowRequestid",
                table: "IssuedBooks",
                column: "borrowRequestid",
                principalTable: "BorrowRequests",
                principalColumn: "borrowRequestid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
