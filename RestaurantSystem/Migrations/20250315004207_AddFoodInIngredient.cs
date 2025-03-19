using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodInIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Foods_FoodId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_FoodId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "Ingredients");

            migrationBuilder.CreateTable(
                name: "FoodIngredient",
                columns: table => new
                {
                    FoodsId = table.Column<long>(type: "bigint", nullable: false),
                    OptionalIngredientsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodIngredient", x => new { x.FoodsId, x.OptionalIngredientsId });
                    table.ForeignKey(
                        name: "FK_FoodIngredient_Foods_FoodsId",
                        column: x => x.FoodsId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodIngredient_Ingredients_OptionalIngredientsId",
                        column: x => x.OptionalIngredientsId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodIngredient_OptionalIngredientsId",
                table: "FoodIngredient",
                column: "OptionalIngredientsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodIngredient");

            migrationBuilder.AddColumn<long>(
                name: "FoodId",
                table: "Ingredients",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_FoodId",
                table: "Ingredients",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Foods_FoodId",
                table: "Ingredients",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id");
        }
    }
}
