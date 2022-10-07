using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ReInitialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barang",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(nullable: true),
                    Satuan = table.Column<string>(nullable: true),
                    Stok = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Karyawan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fullname = table.Column<string>(nullable: true),
                    Alamat = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karyawan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiwayatPengadaan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barang_Id = table.Column<int>(nullable: false),
                    Tanggal = table.Column<string>(nullable: true),
                    Jumlah = table.Column<int>(nullable: false),
                    Harga = table.Column<double>(nullable: false),
                    Supplier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiwayatPengadaan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiwayatPengadaan_Barang_Barang_Id",
                        column: x => x.Barang_Id,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiwayatPerbaikan",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barang_Id = table.Column<int>(nullable: false),
                    Keterangan = table.Column<string>(nullable: true),
                    Biaya = table.Column<double>(nullable: false),
                    Jumlah = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Tanggal_Terima = table.Column<DateTime>(nullable: false),
                    Tanggal_Selesai = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiwayatPerbaikan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiwayatPerbaikan_Barang_Barang_Id",
                        column: x => x.Barang_Id,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiwayatPeminjaman",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barang_Id = table.Column<int>(nullable: false),
                    Karyawan_Id = table.Column<int>(nullable: false),
                    Tanggal_Pinjam = table.Column<DateTime>(nullable: false),
                    Tanggal_Kembali = table.Column<DateTime>(nullable: false),
                    Jumlah = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiwayatPeminjaman", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiwayatPeminjaman_Barang_Barang_Id",
                        column: x => x.Barang_Id,
                        principalTable: "Barang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiwayatPeminjaman_Karyawan_Karyawan_Id",
                        column: x => x.Karyawan_Id,
                        principalTable: "Karyawan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Password = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Karyawan_Id",
                        column: x => x.Id,
                        principalTable: "Karyawan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(nullable: false),
                    Role_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiwayatPeminjaman_Barang_Id",
                table: "RiwayatPeminjaman",
                column: "Barang_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RiwayatPeminjaman_Karyawan_Id",
                table: "RiwayatPeminjaman",
                column: "Karyawan_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RiwayatPengadaan_Barang_Id",
                table: "RiwayatPengadaan",
                column: "Barang_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RiwayatPerbaikan_Barang_Id",
                table: "RiwayatPerbaikan",
                column: "Barang_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Role_Id",
                table: "UserRole",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_User_Id",
                table: "UserRole",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiwayatPeminjaman");

            migrationBuilder.DropTable(
                name: "RiwayatPengadaan");

            migrationBuilder.DropTable(
                name: "RiwayatPerbaikan");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Barang");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Karyawan");
        }
    }
}
