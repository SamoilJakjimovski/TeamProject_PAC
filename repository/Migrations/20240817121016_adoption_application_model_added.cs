using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace repository.Migrations
{
    /// <inheritdoc />
    public partial class adoption_application_model_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdoptionApplication",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdoptionApplicationStatus = table.Column<int>(type: "int", nullable: false),
                    AdopterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Question1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question10 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdoptionApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdoptionApplication_AspNetUsers_AdopterId",
                        column: x => x.AdopterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdoptionApplication_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplication_AdopterId",
                table: "AdoptionApplication",
                column: "AdopterId");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplication_PetId",
                table: "AdoptionApplication",
                column: "PetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdoptionApplication");
        }
    }
}
