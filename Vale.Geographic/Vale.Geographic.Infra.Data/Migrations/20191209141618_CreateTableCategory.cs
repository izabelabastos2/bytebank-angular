using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vale.Geographic.Infra.Data.Migrations
{
    public partial class CreateTableCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Segment_Route_RouteId",
                table: "Segment");

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "Segment",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "Route",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "PointOfInterest",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "PointOfInterest",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Area",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categorys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    TypeEntitie = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorys", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointOfInterest_CategoryId",
                table: "PointOfInterest",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Area_CategoryId",
                table: "Area",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_Categorys_CategoryId",
                table: "Area",
                column: "CategoryId",
                principalTable: "Categorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterest_Categorys_CategoryId",
                table: "PointOfInterest",
                column: "CategoryId",
                principalTable: "Categorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Segment_Route_RouteId",
                table: "Segment",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_Categorys_CategoryId",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterest_Categorys_CategoryId",
                table: "PointOfInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_Segment_Route_RouteId",
                table: "Segment");

            migrationBuilder.DropTable(
                name: "Categorys");

            migrationBuilder.DropIndex(
                name: "IX_PointOfInterest_CategoryId",
                table: "PointOfInterest");

            migrationBuilder.DropIndex(
                name: "IX_Area_CategoryId",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PointOfInterest");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Area");

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "Segment",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "Route",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "AreaId",
                table: "PointOfInterest",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Segment_Route_RouteId",
                table: "Segment",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
