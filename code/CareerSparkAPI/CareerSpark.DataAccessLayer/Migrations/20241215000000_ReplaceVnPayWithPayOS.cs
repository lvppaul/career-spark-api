using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerSpark.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceVnPayWithPayOS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VnPayTransactionId",
                table: "Orders",
                newName: "PayOSTransactionId");

            migrationBuilder.RenameColumn(
                name: "VnPayResponseCode",
                table: "Orders",
                newName: "PayOSResponseCode");

            migrationBuilder.RenameColumn(
                name: "VnPayOrderInfo",
                table: "Orders",
                newName: "PayOSOrderInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayOSTransactionId",
                table: "Orders",
                newName: "VnPayTransactionId");

            migrationBuilder.RenameColumn(
                name: "PayOSResponseCode",
                table: "Orders",
                newName: "VnPayResponseCode");

            migrationBuilder.RenameColumn(
                name: "PayOSOrderInfo",
                table: "Orders",
                newName: "VnPayOrderInfo");
        }
    }
}