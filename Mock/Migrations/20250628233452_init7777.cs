using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class init7777 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductForDietTypes_DietTypes_dietTypeCode",
                table: "ProductForDietTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductForDietTypes_dietTypeCode",
                table: "ProductForDietTypes");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFoodPreferences_ProductId1",
                table: "CustomerFoodPreferences");

            migrationBuilder.DropColumn(
                name: "ProdName",
                table: "ProductForDietTypes");

            migrationBuilder.DropColumn(
                name: "dietTypeCode",
                table: "ProductForDietTypes");

            migrationBuilder.AddColumn<bool>(
                name: "IsAllowed",
                table: "ProductForDietTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductForDietTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductForDietTypes_DietTypeId",
                table: "ProductForDietTypes",
                column: "DietTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductForDietTypes_ProductId",
                table: "ProductForDietTypes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFoodPreferences_ProductId1",
                table: "CustomerFoodPreferences",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductForDietTypes_DietTypes_DietTypeId",
                table: "ProductForDietTypes",
                column: "DietTypeId",
                principalTable: "DietTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductForDietTypes_Products_ProductId",
                table: "ProductForDietTypes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductForDietTypes_DietTypes_DietTypeId",
                table: "ProductForDietTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductForDietTypes_Products_ProductId",
                table: "ProductForDietTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductForDietTypes_DietTypeId",
                table: "ProductForDietTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductForDietTypes_ProductId",
                table: "ProductForDietTypes");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFoodPreferences_ProductId1",
                table: "CustomerFoodPreferences");

            migrationBuilder.DropColumn(
                name: "IsAllowed",
                table: "ProductForDietTypes");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductForDietTypes");

            migrationBuilder.AddColumn<string>(
                name: "ProdName",
                table: "ProductForDietTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "dietTypeCode",
                table: "ProductForDietTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductForDietTypes_dietTypeCode",
                table: "ProductForDietTypes",
                column: "dietTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFoodPreferences_ProductId1",
                table: "CustomerFoodPreferences",
                column: "ProductId1",
                unique: true,
                filter: "[ProductId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductForDietTypes_DietTypes_dietTypeCode",
                table: "ProductForDietTypes",
                column: "dietTypeCode",
                principalTable: "DietTypes",
                principalColumn: "Id");
        }
    }
}
