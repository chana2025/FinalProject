using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerFoodPreferenceRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerFoodPreferences_ProductId",
                table: "CustomerFoodPreferences");

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "CustomerFoodPreferences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFoodPreferences_ProductId",
                table: "CustomerFoodPreferences",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFoodPreferences_ProductId1",
                table: "CustomerFoodPreferences",
                column: "ProductId1",
                unique: true,
                filter: "[ProductId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFoodPreferences_Products_ProductId1",
                table: "CustomerFoodPreferences",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFoodPreferences_Products_ProductId1",
                table: "CustomerFoodPreferences");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFoodPreferences_ProductId",
                table: "CustomerFoodPreferences");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFoodPreferences_ProductId1",
                table: "CustomerFoodPreferences");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "CustomerFoodPreferences");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFoodPreferences_ProductId",
                table: "CustomerFoodPreferences",
                column: "ProductId",
                unique: true);
        }
    }
}
