using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class removedivisionkaryawan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Divisi",
                table: "Karyawan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Divisi",
                table: "Karyawan",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
