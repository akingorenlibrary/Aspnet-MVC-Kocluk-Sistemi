using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kocluk.Migrations
{
    public partial class InitialCreate14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DanisanSayisi",
                table: "Kullanicilar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Deneyimleri",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hedefleri",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SifreSifirlamaMetni",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UzmanlikAlanlari",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DanisanBeslenmeProgramlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hedef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GunlukOgunler = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KaloriHedefi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DanisanId = table.Column<int>(type: "int", nullable: false),
                    AntrenorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanisanBeslenmeProgramlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanisanBeslenmeProgramlari_Kullanicilar_AntrenorId",
                        column: x => x.AntrenorId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanisanBeslenmeProgramlari_Kullanicilar_DanisanId",
                        column: x => x.DanisanId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanisanEgzersizProgramlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EgzersizAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hedefleri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetSayisi = table.Column<int>(type: "int", nullable: false),
                    TekrarSayisi = table.Column<int>(type: "int", nullable: false),
                    VideoRehberi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrograminSuresi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramaBaslamaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DanisanId = table.Column<int>(type: "int", nullable: false),
                    AntrenorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanisanEgzersizProgramlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanisanEgzersizProgramlari_Kullanicilar_AntrenorId",
                        column: x => x.AntrenorId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanisanEgzersizProgramlari_Kullanicilar_DanisanId",
                        column: x => x.DanisanId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IlerlemeDurumlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kilo = table.Column<int>(type: "int", nullable: false),
                    Boy = table.Column<int>(type: "int", nullable: false),
                    VucutYagOrani = table.Column<int>(type: "int", nullable: false),
                    KasKutlesi = table.Column<int>(type: "int", nullable: false),
                    VucutKitleIndeksi = table.Column<int>(type: "int", nullable: false),
                    KullaniciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IlerlemeDurumlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IlerlemeDurumlari_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KullanicilarinAntrenorleris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    AntrenorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullanicilarinAntrenorleris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullanicilarinAntrenorleris_Kullanicilar_AntrenorId",
                        column: x => x.AntrenorId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KullanicilarinAntrenorleris_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mesajlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AliciId = table.Column<int>(type: "int", nullable: false),
                    GonderenId = table.Column<int>(type: "int", nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tarih = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesajlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mesajlar_Kullanicilar_AliciId",
                        column: x => x.AliciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mesajlar_Kullanicilar_GonderenId",
                        column: x => x.GonderenId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanisanBeslenmeProgramlari_AntrenorId",
                table: "DanisanBeslenmeProgramlari",
                column: "AntrenorId");

            migrationBuilder.CreateIndex(
                name: "IX_DanisanBeslenmeProgramlari_DanisanId",
                table: "DanisanBeslenmeProgramlari",
                column: "DanisanId");

            migrationBuilder.CreateIndex(
                name: "IX_DanisanEgzersizProgramlari_AntrenorId",
                table: "DanisanEgzersizProgramlari",
                column: "AntrenorId");

            migrationBuilder.CreateIndex(
                name: "IX_DanisanEgzersizProgramlari_DanisanId",
                table: "DanisanEgzersizProgramlari",
                column: "DanisanId");

            migrationBuilder.CreateIndex(
                name: "IX_IlerlemeDurumlari_KullaniciId",
                table: "IlerlemeDurumlari",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_KullanicilarinAntrenorleris_AntrenorId",
                table: "KullanicilarinAntrenorleris",
                column: "AntrenorId");

            migrationBuilder.CreateIndex(
                name: "IX_KullanicilarinAntrenorleris_KullaniciId",
                table: "KullanicilarinAntrenorleris",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_AliciId",
                table: "Mesajlar",
                column: "AliciId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_GonderenId",
                table: "Mesajlar",
                column: "GonderenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanisanBeslenmeProgramlari");

            migrationBuilder.DropTable(
                name: "DanisanEgzersizProgramlari");

            migrationBuilder.DropTable(
                name: "IlerlemeDurumlari");

            migrationBuilder.DropTable(
                name: "KullanicilarinAntrenorleris");

            migrationBuilder.DropTable(
                name: "Mesajlar");

            migrationBuilder.DropColumn(
                name: "DanisanSayisi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Deneyimleri",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Hedefleri",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "SifreSifirlamaMetni",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "UzmanlikAlanlari",
                table: "Kullanicilar");
        }
    }
}
