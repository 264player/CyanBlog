using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyanBlog.Migrations
{
    /// <inheritdoc />
    public partial class CommentsGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Blog_BlogId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "BlogId",
                table: "Comment",
                newName: "BlogID");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_BlogId",
                table: "Comment",
                newName: "IX_Comment_BlogID");

            migrationBuilder.AlterColumn<uint>(
                name: "ManagerId",
                table: "Message",
                type: "int unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<uint>(
                name: "ManagerId",
                table: "Comment",
                type: "int unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<uint>(
                name: "FatherID",
                table: "Comment",
                type: "int unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<uint>(
                name: "BlogID",
                table: "Comment",
                type: "int unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<uint>(
                name: "BlogID",
                table: "Blog",
                type: "int unsigned",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_FatherID",
                table: "Comment",
                column: "FatherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Blog_BlogID",
                table: "Comment",
                column: "BlogID",
                principalTable: "Blog",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_FatherID",
                table: "Comment",
                column: "FatherID",
                principalTable: "Comment",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Blog_BlogID",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_FatherID",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_FatherID",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "BlogID",
                table: "Comment",
                newName: "BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_BlogID",
                table: "Comment",
                newName: "IX_Comment_BlogId");

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "Message",
                type: "int",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "int unsigned");

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "Comment",
                type: "int",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "int unsigned");

            migrationBuilder.AlterColumn<int>(
                name: "FatherID",
                table: "Comment",
                type: "int",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "int unsigned");

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "Comment",
                type: "int",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "int unsigned");

            migrationBuilder.AlterColumn<int>(
                name: "BlogID",
                table: "Blog",
                type: "int",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "int unsigned")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Blog_BlogId",
                table: "Comment",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "BlogID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
