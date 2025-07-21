using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgizDisSagligiTakip.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmailDogrulama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DogrulamaKodu",
                table: "Kullanicilar",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DogrulamaKoduSuresi",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailDogrulandi",
                table: "Kullanicilar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DogrulamaKodu",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "DogrulamaKoduSuresi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "EmailDogrulandi",
                table: "Kullanicilar");
        }
    }
}
