using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServerAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Description", "ItemType", "Name" },
                values: new object[,]
                {
                    { 1, "기본 지급되는 낡은 검입니다.", "Weapon", "초보자용 검" },
                    { 2, "체력을 50 회복합니다.", "Consumable", "회복 포션" },
                    { 3, "새로운 캐릭터를 모집할 수 있는 티켓입니다.", "Currency", "특별 채용 티켓" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
