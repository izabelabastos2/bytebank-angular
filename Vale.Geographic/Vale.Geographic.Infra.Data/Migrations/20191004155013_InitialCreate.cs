using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace Vale.Geographic.Infra.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonSamples");

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Location = table.Column<Geometry>(type: "geography", nullable: false),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Area_Area_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PointOfInterest",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Location = table.Column<Point>(type: "geography", nullable: false),
                    AreaId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointOfInterest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointOfInterest_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Length = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Location = table.Column<Geometry>(type: "geography", nullable: false),
                    AreaId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Route_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Segment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Length = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Location = table.Column<Geometry>(type: "geography", nullable: false),
                    RouteId = table.Column<Guid>(nullable: false),
                    AreaId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Segment_Area_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Segment_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Area_ParentId",
                table: "Area",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfInterest_AreaId",
                table: "PointOfInterest",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Route_AreaId",
                table: "Route",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Segment_AreaId",
                table: "Segment",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Segment_RouteId",
                table: "Segment",
                column: "RouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointOfInterest");

            migrationBuilder.DropTable(
                name: "Segment");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.CreateTable(
                name: "PersonSamples",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    DateBirth = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSamples", x => x.Id);
                });
        }
    }
}
