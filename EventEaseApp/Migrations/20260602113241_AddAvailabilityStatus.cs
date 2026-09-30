using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEaseApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAvailabilityStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_AVAILABLE",
                table: "Venue",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IS_AVAILABLE",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_AVAILABLE",
                table: "Venue");

            migrationBuilder.DropColumn(
                name: "IS_AVAILABLE",
                table: "Bookings");
        }
    }
}
