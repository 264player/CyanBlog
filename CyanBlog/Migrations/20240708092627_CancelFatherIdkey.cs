using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyanBlog.Migrations
{
    /// <inheritdoc />
    public partial class CancelFatherIdkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_FatherID",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_FatherID",
                table: "Comment");

            migrationBuilder.AddColumn<uint>(
                name: "FatherCommentCommentId",
                table: "Comment",
                type: "int unsigned",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_FatherCommentCommentId",
                table: "Comment",
                column: "FatherCommentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_FatherCommentCommentId",
                table: "Comment",
                column: "FatherCommentCommentId",
                principalTable: "Comment",
                principalColumn: "CommentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_FatherCommentCommentId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_FatherCommentCommentId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "FatherCommentCommentId",
                table: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_FatherID",
                table: "Comment",
                column: "FatherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_FatherID",
                table: "Comment",
                column: "FatherID",
                principalTable: "Comment",
                principalColumn: "CommentId");
        }
    }
}
