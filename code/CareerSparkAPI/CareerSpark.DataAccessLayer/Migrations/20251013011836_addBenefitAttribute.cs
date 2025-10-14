using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSpark.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addBenefitAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Benefits",
                table: "SubscriptionPlan",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Benefits",
                table: "SubscriptionPlan");
        }
    }
}
