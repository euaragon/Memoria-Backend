using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemoriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ArregloDeRelaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contenidos_Secciones_IdSeccion",
                table: "Contenidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Secciones_Paginas_IdPagina",
                table: "Secciones");

            migrationBuilder.DropIndex(
                name: "IX_Secciones_IdPagina",
                table: "Secciones");

            migrationBuilder.DropIndex(
                name: "IX_Secciones_Url",
                table: "Secciones");

            migrationBuilder.RenameColumn(
                name: "IdPagina",
                table: "Secciones",
                newName: "Anio");

            migrationBuilder.RenameColumn(
                name: "IdSeccion",
                table: "Contenidos",
                newName: "PaginaId");

            migrationBuilder.RenameIndex(
                name: "IX_Contenidos_IdSeccion",
                table: "Contenidos",
                newName: "IX_Contenidos_PaginaId");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Secciones",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "IconoCss",
                table: "Secciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreEnsamblado",
                table: "Secciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeccionId",
                table: "Paginas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_Url",
                table: "Secciones",
                column: "Url",
                unique: true,
                filter: "[Url] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Paginas_SeccionId",
                table: "Paginas",
                column: "SeccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contenidos_Paginas_PaginaId",
                table: "Contenidos",
                column: "PaginaId",
                principalTable: "Paginas",
                principalColumn: "IdPagina",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Paginas_Secciones_SeccionId",
                table: "Paginas",
                column: "SeccionId",
                principalTable: "Secciones",
                principalColumn: "IdSeccion",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contenidos_Paginas_PaginaId",
                table: "Contenidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Paginas_Secciones_SeccionId",
                table: "Paginas");

            migrationBuilder.DropIndex(
                name: "IX_Secciones_Url",
                table: "Secciones");

            migrationBuilder.DropIndex(
                name: "IX_Paginas_SeccionId",
                table: "Paginas");

            migrationBuilder.DropColumn(
                name: "IconoCss",
                table: "Secciones");

            migrationBuilder.DropColumn(
                name: "NombreEnsamblado",
                table: "Secciones");

            migrationBuilder.DropColumn(
                name: "SeccionId",
                table: "Paginas");

            migrationBuilder.RenameColumn(
                name: "Anio",
                table: "Secciones",
                newName: "IdPagina");

            migrationBuilder.RenameColumn(
                name: "PaginaId",
                table: "Contenidos",
                newName: "IdSeccion");

            migrationBuilder.RenameIndex(
                name: "IX_Contenidos_PaginaId",
                table: "Contenidos",
                newName: "IX_Contenidos_IdSeccion");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Secciones",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_IdPagina",
                table: "Secciones",
                column: "IdPagina");

            migrationBuilder.CreateIndex(
                name: "IX_Secciones_Url",
                table: "Secciones",
                column: "Url",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contenidos_Secciones_IdSeccion",
                table: "Contenidos",
                column: "IdSeccion",
                principalTable: "Secciones",
                principalColumn: "IdSeccion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Secciones_Paginas_IdPagina",
                table: "Secciones",
                column: "IdPagina",
                principalTable: "Paginas",
                principalColumn: "IdPagina",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
