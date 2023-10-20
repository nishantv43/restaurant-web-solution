using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Services.RewardsAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRewardsTableNameinDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Rewards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rewards",
                table: "Rewards",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rewards",
                table: "Rewards");

            migrationBuilder.RenameTable(
                name: "Rewards",
                newName: "Orders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");
        }
    }
}
