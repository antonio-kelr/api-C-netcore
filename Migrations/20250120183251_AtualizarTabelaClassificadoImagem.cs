using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiNet.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarTabelaClassificadoImagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_classificado_imagem_classificado_cobertura_id",
                table: "classificado_imagem");

            migrationBuilder.RenameColumn(
                name: "cobertura_id",
                table: "classificado_imagem",
                newName: "classificado_id");

            migrationBuilder.RenameIndex(
                name: "IX_classificado_imagem_cobertura_id",
                table: "classificado_imagem",
                newName: "IX_classificado_imagem_classificado_id");

            migrationBuilder.AddForeignKey(
                name: "FK_classificado_imagem_classificado_classificado_id",
                table: "classificado_imagem",
                column: "classificado_id",
                principalTable: "classificado",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_classificado_imagem_classificado_classificado_id",
                table: "classificado_imagem");

            migrationBuilder.RenameColumn(
                name: "classificado_id",
                table: "classificado_imagem",
                newName: "cobertura_id");

            migrationBuilder.RenameIndex(
                name: "IX_classificado_imagem_classificado_id",
                table: "classificado_imagem",
                newName: "IX_classificado_imagem_cobertura_id");

            migrationBuilder.AddForeignKey(
                name: "FK_classificado_imagem_classificado_cobertura_id",
                table: "classificado_imagem",
                column: "cobertura_id",
                principalTable: "classificado",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
