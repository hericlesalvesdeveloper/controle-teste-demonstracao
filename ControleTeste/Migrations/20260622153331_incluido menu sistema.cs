using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleTeste.Migrations
{
    /// <inheritdoc />
    public partial class incluidomenusistema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MenuSistema",
                table: "tb_alteracao",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuSistema",
                table: "tb_alteracao");
        }
    }
}
