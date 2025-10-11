using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSpark.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addtagatBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tag",
                table: "Blogs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tag",
                table: "Blogs");
        }
    }
}
