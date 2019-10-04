using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vale.Geographic.Infra.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "PersonSamples",
                table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>("varchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>("varchar(50)", maxLength: 50, nullable: false),
                    DateBirth = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_PersonSamples", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PersonSamples");
        }
    }
}