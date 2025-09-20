using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSpark.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionPlanAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "SubscriptionPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "SubscriptionPlan");
        }
    }
}
