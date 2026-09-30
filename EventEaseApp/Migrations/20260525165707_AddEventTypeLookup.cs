using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventEaseApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTypeLookup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EVENTTYPE_ID",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    EVENTTYPE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EVENTTYPE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.EVENTTYPE_ID);
                });

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "EVENTTYPE_ID", "EVENTTYPE_NAME" },
                values: new object[,]
                {
                    { 1, "Conference" },
                    { 2, "Wedding" },
                    { 3, "Concert" },
                    { 4, "Corporate" },
                    { 5, "Birthday" },
                    { 6, "Exhibition" },
                    { 7, "Sports" },
                    { 8, "Festival" },
                    { 9, "Anniversary" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_EVENTTYPE_ID",
                table: "Events",
                column: "EVENTTYPE_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventTypes_EVENTTYPE_ID",
                table: "Events",
                column: "EVENTTYPE_ID",
                principalTable: "EventTypes",
                principalColumn: "EVENTTYPE_ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventTypes_EVENTTYPE_ID",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropIndex(
                name: "IX_Events_EVENTTYPE_ID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EVENTTYPE_ID",
                table: "Events");
        }
    }
}
