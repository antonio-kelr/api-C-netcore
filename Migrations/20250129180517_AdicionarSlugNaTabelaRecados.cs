using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiNet.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarSlugNaTabelaRecados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "recado",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "slug",
                table: "recado");
        }
    }
}
