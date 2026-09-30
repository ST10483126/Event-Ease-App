using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEaseApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherToEventTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "EVENTTYPE_ID", "EVENTTYPE_NAME" },
                values: new object[] { 10, "Other" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTypes",
                keyColumn: "EVENTTYPE_ID",
                keyValue: 10);
        }
    }
}
