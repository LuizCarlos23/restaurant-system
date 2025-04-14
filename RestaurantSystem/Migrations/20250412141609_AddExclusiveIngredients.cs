using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddExclusiveIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodIngredient_Foods_FoodsId",
                table: "FoodIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodIngredient_Ingredients_OptionalIngredientsId",
                table: "FoodIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoodIngredient",
                table: "FoodIngredient");

            migrationBuilder.RenameTable(
                name: "FoodIngredient",
                newName: "FoodOptionalIngredients");

            migrationBuilder.RenameColumn(
                name: "FoodsId",
                table: "FoodOptionalIngredients",
                newName: "OptionalForFoodsId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodIngredient_OptionalIngredientsId",
                table: "FoodOptionalIngredients",
                newName: "IX_FoodOptionalIngredients_OptionalIngredientsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoodOptionalIngredients",
                table: "FoodOptionalIngredients",
                columns: new[] { "OptionalForFoodsId", "OptionalIngredientsId" });

            migrationBuilder.CreateTable(
                name: "FoodExclusiveIngredients",
                columns: table => new
                {
                    ExclusiveForFoodsId = table.Column<long>(type: "bigint", nullable: false),
                    ExclusiveIngredientsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodExclusiveIngredients", x => new { x.ExclusiveForFoodsId, x.ExclusiveIngredientsId });
                    table.ForeignKey(
                        name: "FK_FoodExclusiveIngredients_Foods_ExclusiveForFoodsId",
                        column: x => x.ExclusiveForFoodsId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodExclusiveIngredients_Ingredients_ExclusiveIngredientsId",
                        column: x => x.ExclusiveIngredientsId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodExclusiveIngredients_ExclusiveIngredientsId",
                table: "FoodExclusiveIngredients",
                column: "ExclusiveIngredientsId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodOptionalIngredients_Foods_OptionalForFoodsId",
                table: "FoodOptionalIngredients",
                column: "OptionalForFoodsId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodOptionalIngredients_Ingredients_OptionalIngredientsId",
                table: "FoodOptionalIngredients",
                column: "OptionalIngredientsId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodOptionalIngredients_Foods_OptionalForFoodsId",
                table: "FoodOptionalIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodOptionalIngredients_Ingredients_OptionalIngredientsId",
                table: "FoodOptionalIngredients");

            migrationBuilder.DropTable(
                name: "FoodExclusiveIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoodOptionalIngredients",
                table: "FoodOptionalIngredients");

            migrationBuilder.RenameTable(
                name: "FoodOptionalIngredients",
                newName: "FoodIngredient");

            migrationBuilder.RenameColumn(
                name: "OptionalForFoodsId",
                table: "FoodIngredient",
                newName: "FoodsId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodOptionalIngredients_OptionalIngredientsId",
                table: "FoodIngredient",
                newName: "IX_FoodIngredient_OptionalIngredientsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoodIngredient",
                table: "FoodIngredient",
                columns: new[] { "FoodsId", "OptionalIngredientsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FoodIngredient_Foods_FoodsId",
                table: "FoodIngredient",
                column: "FoodsId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodIngredient_Ingredients_OptionalIngredientsId",
                table: "FoodIngredient",
                column: "OptionalIngredientsId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
