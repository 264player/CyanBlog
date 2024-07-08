using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyanBlog.Migrations
{
    /// <inheritdoc />
    public partial class UpDateMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Message");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "BlogId",
                table: "Message",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);
        }
    }
}
