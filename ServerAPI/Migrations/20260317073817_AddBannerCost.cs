using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBannerCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cost",
                table: "gacha_banners",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cost",
                table: "gacha_banners");
        }
    }
}
