using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionalAndExclusiveIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_OrderedItems_OrderItemId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_OrderItemId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "Ingredients");

            migrationBuilder.AddColumn<long>(
                name: "ExclusiveIngredientSelectedId",
                table: "OrderedItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderItemOptinalIngredients",
                columns: table => new
                {
                    OptionalIngredientsSelectedId = table.Column<long>(type: "bigint", nullable: false),
                    OrderItemId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemOptinalIngredients", x => new { x.OptionalIngredientsSelectedId, x.OrderItemId });
                    table.ForeignKey(
                        name: "FK_OrderItemOptinalIngredients_Ingredients_OptionalIngredientsSelectedId",
                        column: x => x.OptionalIngredientsSelectedId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemOptinalIngredients_OrderedItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderedItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_ExclusiveIngredientSelectedId",
                table: "OrderedItems",
                column: "ExclusiveIngredientSelectedId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOptinalIngredients_OrderItemId",
                table: "OrderItemOptinalIngredients",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_Ingredients_ExclusiveIngredientSelectedId",
                table: "OrderedItems",
                column: "ExclusiveIngredientSelectedId",
                principalTable: "Ingredients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_Ingredients_ExclusiveIngredientSelectedId",
                table: "OrderedItems");

            migrationBuilder.DropTable(
                name: "OrderItemOptinalIngredients");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_ExclusiveIngredientSelectedId",
                table: "OrderedItems");

            migrationBuilder.DropColumn(
                name: "ExclusiveIngredientSelectedId",
                table: "OrderedItems");

            migrationBuilder.AddColumn<long>(
                name: "OrderItemId",
                table: "Ingredients",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_OrderItemId",
                table: "Ingredients",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_OrderedItems_OrderItemId",
                table: "Ingredients",
                column: "OrderItemId",
                principalTable: "OrderedItems",
                principalColumn: "Id");
        }
    }
}
