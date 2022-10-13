using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class fieldkaryawan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Departemen",
                table: "Karyawan",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Divisi",
                table: "Karyawan",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Departemen",
                table: "Karyawan");

            migrationBuilder.DropColumn(
                name: "Divisi",
                table: "Karyawan");
        }
    }
}
