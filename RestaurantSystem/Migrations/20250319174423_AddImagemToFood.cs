using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddImagemToFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Foods",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "Foods");
        }
    }
}
