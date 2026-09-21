using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisGuinchos.Migrations
{
    /// <inheritdoc />
    public partial class GuinchoFotoPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Foto",
                table: "Guinchos",
                newName: "FotoPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FotoPath",
                table: "Guinchos",
                newName: "Foto");
        }
    }
}
