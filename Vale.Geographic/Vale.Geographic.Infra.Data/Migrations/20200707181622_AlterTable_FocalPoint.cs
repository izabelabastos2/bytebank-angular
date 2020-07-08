using Microsoft.EntityFrameworkCore.Migrations;

namespace Vale.Geographic.Infra.Data.Migrations
{
    public partial class AlterTable_FocalPoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "FocalPoints",
                type: "varchar(75)",
                maxLength: 75,
                nullable: true,
                oldClrType: typeof(long),
                oldMaxLength: 20,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PhoneNumber",
                table: "FocalPoints",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(75)",
                oldMaxLength: 75,
                oldNullable: true);
        }
    }
}
