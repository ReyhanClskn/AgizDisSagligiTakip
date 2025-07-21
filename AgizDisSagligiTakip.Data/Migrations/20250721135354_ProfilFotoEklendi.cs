using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgizDisSagligiTakip.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProfilFotoEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilFotoUrl",
                table: "Kullanicilar",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilFotoUrl",
                table: "Kullanicilar");
        }
    }
}
