using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgizDisSagligiTakip.Data.Migrations
{
    /// <inheritdoc />
    public partial class OneriSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Oneriler",
                columns: new[] { "Id", "Aktif", "Metin", "OlusturmaTarihi" },
                values: new object[,]
                {
                    { 1, true, "Günde en az 2 kez diş fırçalamayı unutmayın!", new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(7915) },
                    { 2, true, "Diş ipi kullanmak, diş fırçasının ulaşamadığı yerleri temizler.", new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8114) },
                    { 3, true, "Şekerli içecekleri sınırlandırın, dişleriniz size teşekkür edecek.", new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8115) },
                    { 4, true, "6 ayda bir diş hekimi kontrolü yaptırmayı ihmal etmeyin.", new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8116) },
                    { 5, true, "Ağız gargarası kullanarak ağız hijyeninizi destekleyin.", new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8117) },
                    { 6, true, "Diş fırçanızı 3 ayda bir değiştirin.", new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8118) },
                    { 7, true, "Sert fırçalama yerine nazik dairesel hareketler yapın.", new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8119) },
                    { 8, true, "Yemek sonrası 30 dakika bekleyip diş fırçalayın.", new DateTime(2025, 7, 10, 15, 34, 2, 11, DateTimeKind.Local).AddTicks(8120) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Oneriler",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
