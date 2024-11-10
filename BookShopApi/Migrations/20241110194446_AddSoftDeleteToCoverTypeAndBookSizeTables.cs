using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToCoverTypeAndBookSizeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "BookSizes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BookSizes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CoverTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoverTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "BookSizes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BookSizes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CoverTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoverTypes");
        }
    }
}
