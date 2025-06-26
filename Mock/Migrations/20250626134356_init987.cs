using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class init987 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealsPerDay",
                table: "DietTypes");

            migrationBuilder.DropColumn(
                name: "NumCalories",
                table: "DietTypes");

            migrationBuilder.DropColumn(
                name: "TimeMealsString",
                table: "DietTypes");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialComments",
                table: "DietTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "Calories",
                table: "DietTypes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Carbohydrates",
                table: "DietTypes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Fat",
                table: "DietTypes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Protein",
                table: "DietTypes",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calories",
                table: "DietTypes");

            migrationBuilder.DropColumn(
                name: "Carbohydrates",
                table: "DietTypes");

            migrationBuilder.DropColumn(
                name: "Fat",
                table: "DietTypes");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "DietTypes");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialComments",
                table: "DietTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MealsPerDay",
                table: "DietTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumCalories",
                table: "DietTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TimeMealsString",
                table: "DietTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
