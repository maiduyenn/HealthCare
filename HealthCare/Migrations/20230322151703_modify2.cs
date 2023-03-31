using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Migrations
{
    public partial class modify2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalActivity_AthleteInformation_AthleteInformationId",
                table: "PhysicalActivity");

            migrationBuilder.DropIndex(
                name: "IX_PhysicalActivity_AthleteInformationId",
                table: "PhysicalActivity");

            migrationBuilder.DropColumn(
                name: "AthleteInformationId",
                table: "PhysicalActivity");

            migrationBuilder.CreateTable(
                name: "AthleteActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    AthleteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AthleteInformationId = table.Column<int>(type: "int", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AthleteActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AthleteActivities_AspNetUsers_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AthleteActivities_AthleteInformation_AthleteInformationId",
                        column: x => x.AthleteInformationId,
                        principalTable: "AthleteInformation",
                        principalColumn: "AthleteInformationId");
                    table.ForeignKey(
                        name: "FK_AthleteActivities_PhysicalActivity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "PhysicalActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AthleteActivities_ActivityId",
                table: "AthleteActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteActivities_AthleteId",
                table: "AthleteActivities",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_AthleteActivities_AthleteInformationId",
                table: "AthleteActivities",
                column: "AthleteInformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AthleteActivities");

            migrationBuilder.AddColumn<int>(
                name: "AthleteInformationId",
                table: "PhysicalActivity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalActivity_AthleteInformationId",
                table: "PhysicalActivity",
                column: "AthleteInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalActivity_AthleteInformation_AthleteInformationId",
                table: "PhysicalActivity",
                column: "AthleteInformationId",
                principalTable: "AthleteInformation",
                principalColumn: "AthleteInformationId");
        }
    }
}
