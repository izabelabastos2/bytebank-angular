using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vale.Geographic.Infra.Data.Migrations
{
    public partial class AlterTable_NotificationAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "NotificationId",
                table: "NotificationAnswers",
                nullable: false,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "NotificationId",
                table: "NotificationAnswers",
                nullable: false,
                oldClrType: typeof(Guid));
        }
    }
}
