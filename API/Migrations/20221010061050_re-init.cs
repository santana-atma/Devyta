using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class reinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Karyawan_Id",
                table: "RiwayatPerbaikan",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RiwayatPerbaikan_Karyawan_Id",
                table: "RiwayatPerbaikan",
                column: "Karyawan_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiwayatPerbaikan_Karyawan_Karyawan_Id",
                table: "RiwayatPerbaikan",
                column: "Karyawan_Id",
                principalTable: "Karyawan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiwayatPerbaikan_Karyawan_Karyawan_Id",
                table: "RiwayatPerbaikan");

            migrationBuilder.DropIndex(
                name: "IX_RiwayatPerbaikan_Karyawan_Id",
                table: "RiwayatPerbaikan");

            migrationBuilder.DropColumn(
                name: "Karyawan_Id",
                table: "RiwayatPerbaikan");
        }
    }
}
