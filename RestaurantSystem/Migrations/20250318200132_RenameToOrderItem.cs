using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSystem.Migrations
{
    /// <inheritdoc />
    public partial class RenameToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_OrderedItems_OrderedItemId",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "OrderedItemId",
                table: "Ingredients",
                newName: "OrderItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_OrderedItemId",
                table: "Ingredients",
                newName: "IX_Ingredients_OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_OrderedItems_OrderItemId",
                table: "Ingredients",
                column: "OrderItemId",
                principalTable: "OrderedItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_OrderedItems_OrderItemId",
                table: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "Ingredients",
                newName: "OrderedItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_OrderItemId",
                table: "Ingredients",
                newName: "IX_Ingredients_OrderedItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_OrderedItems_OrderedItemId",
                table: "Ingredients",
                column: "OrderedItemId",
                principalTable: "OrderedItems",
                principalColumn: "Id");
        }
    }
}
