using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vale.Geographic.Infra.Data.Migrations
{
    public partial class AlterTable_Area_and_PointOfInterest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "PointOfInterest",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "PointOfInterest",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Area",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "PointOfInterest");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Area");

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "PointOfInterest",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
