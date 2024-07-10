using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyanBlog.Migrations
{
    /// <inheritdoc />
    public partial class CommentFatherIDCanBeNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_FatherID",
                table: "Comment");

            migrationBuilder.AlterColumn<uint>(
                name: "FatherID",
                table: "Comment",
                type: "int unsigned",
                nullable: true,
                oldClrType: typeof(uint),
                oldType: "int unsigned");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_FatherID",
                table: "Comment",
                column: "FatherID",
                principalTable: "Comment",
                principalColumn: "CommentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_FatherID",
                table: "Comment");

            migrationBuilder.AlterColumn<uint>(
                name: "FatherID",
                table: "Comment",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u,
                oldClrType: typeof(uint),
                oldType: "int unsigned",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_FatherID",
                table: "Comment",
                column: "FatherID",
                principalTable: "Comment",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
