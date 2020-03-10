using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vale.Geographic.Infra.Data.Migrations
{
    public partial class CreateTable_Auditory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Segment",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Segment",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Route",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Route",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PointOfInterest",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "PointOfInterest",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Categorys",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Categorys",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Area",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Area",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Auditory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "varchar(100)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    TypeEntitie = table.Column<int>(nullable: false),
                    OldValue = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    NewValue = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    AreaId = table.Column<Guid>(nullable: true),
                    PointOfInterestId = table.Column<Guid>(nullable: true),
                    CategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditory_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auditory_Categorys_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auditory_PointOfInterest_PointOfInterestId",
                        column: x => x.PointOfInterestId,
                        principalTable: "PointOfInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditory_AreaId",
                table: "Auditory",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditory_CategoryId",
                table: "Auditory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditory_PointOfInterestId",
                table: "Auditory",
                column: "PointOfInterestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditory");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Segment");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Segment");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PointOfInterest");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "PointOfInterest");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categorys");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Categorys");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Area");
        }
    }
}
