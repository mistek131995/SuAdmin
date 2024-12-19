using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuAdmin.Migrations
{
    /// <inheritdoc />
    public partial class AddInstalledPluginVersionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Plugins",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Plugins");
        }
    }
}
