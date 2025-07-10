using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgizDisSagligiTakip.Data.Migrations
{
    /// <inheritdoc />
    public partial class KullanicilariTemizle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Kullanıcıları temizle
            migrationBuilder.Sql("DELETE FROM Kullanicilar");
            
            // Identity'yi sıfırla
            migrationBuilder.Sql("DBCC CHECKIDENT ('Kullanicilar', RESEED, 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
