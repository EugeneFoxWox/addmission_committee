using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace selection_committee.Migrations
{
    /// <inheritdoc />
    public partial class asdfasdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entrants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Surname = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Patronymic = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Age = table.Column<int>(type: "INTEGER", nullable: true),
                    Citizenship = table.Column<string>(type: "TEXT", nullable: true),
                    Subject = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    District = table.Column<string>(type: "TEXT", nullable: true),
                    Finished9Or11Grade = table.Column<string>(type: "TEXT", nullable: true),
                    OtherSchoolsAttended = table.Column<string>(type: "TEXT", nullable: true),
                    AverageScore = table.Column<float>(type: "REAL", nullable: true),
                    SNILS = table.Column<string>(type: "TEXT", nullable: true),
                    HasDisabilityCertificate = table.Column<string>(type: "TEXT", nullable: true),
                    DisabilityCertificateScan = table.Column<byte[]>(type: "BLOB", nullable: true),
                    HasOrphanageDocuments = table.Column<string>(type: "TEXT", nullable: true),
                    OrphanageDocumentsScan = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Speciality = table.Column<string>(type: "TEXT", nullable: true),
                    AdmissionRulesLink = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrants", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entrants");
        }
    }
}
