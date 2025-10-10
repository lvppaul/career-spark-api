using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSpark.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class tagtoTagatBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tag",
                table: "Blogs",
                newName: "Tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "Blogs",
                newName: "tag");
        }
    }
}
