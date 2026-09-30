using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEaseApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEventEaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Event_EventsId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Venue_VenueId",
                table: "Booking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Venuename",
                table: "Venue",
                newName: "VENUENAME");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Venue",
                newName: "LOCATION");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Venue",
                newName: "CAPACITY");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Venue",
                newName: "ADDRESS");

            migrationBuilder.RenameColumn(
                name: "Venueid",
                table: "Venue",
                newName: "VENUEID");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Venue",
                newName: "IMAGE_URL");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Events",
                newName: "STATUS");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Events",
                newName: "LOCATION");

            migrationBuilder.RenameColumn(
                name: "EventTime",
                table: "Events",
                newName: "EVENT_TIME");

            migrationBuilder.RenameColumn(
                name: "EventName",
                table: "Events",
                newName: "EVENT_NAME");

            migrationBuilder.RenameColumn(
                name: "EventEndTime",
                table: "Events",
                newName: "EVENT_END_TIME");

            migrationBuilder.RenameColumn(
                name: "EventDescription",
                table: "Events",
                newName: "EVENT_DESCRIPTION");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Events",
                newName: "EVENT_ID");

            migrationBuilder.RenameColumn(
                name: "Bookingreference",
                table: "Bookings",
                newName: "BOOKINGREFERENCE");

            migrationBuilder.RenameColumn(
                name: "Bookingid",
                table: "Bookings",
                newName: "BOOKINGID");

            migrationBuilder.RenameColumn(
                name: "VenueId",
                table: "Bookings",
                newName: "VENUE_ID");

            migrationBuilder.RenameColumn(
                name: "EventsId",
                table: "Bookings",
                newName: "EVENTS_ID");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Bookings",
                newName: "CUSTOMER_NAME");

            migrationBuilder.RenameColumn(
                name: "CustomerContact",
                table: "Bookings",
                newName: "CUSTOMER_CONTACT");

            migrationBuilder.RenameColumn(
                name: "BookingStatus",
                table: "Bookings",
                newName: "BOOKING_STATUS");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "Bookings",
                newName: "BOOKING_DATE");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_VenueId",
                table: "Bookings",
                newName: "IX_Bookings_VENUE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_EventsId",
                table: "Bookings",
                newName: "IX_Bookings_EVENTS_ID");

            migrationBuilder.AlterColumn<int>(
                name: "VENUE_ID",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EVENTS_ID",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "EVENT_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BOOKINGID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Events_EVENTS_ID",
                table: "Bookings",
                column: "EVENTS_ID",
                principalTable: "Events",
                principalColumn: "EVENT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Venue_VENUE_ID",
                table: "Bookings",
                column: "VENUE_ID",
                principalTable: "Venue",
                principalColumn: "VENUEID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Events_EVENTS_ID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Venue_VENUE_ID",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameColumn(
                name: "VENUENAME",
                table: "Venue",
                newName: "Venuename");

            migrationBuilder.RenameColumn(
                name: "LOCATION",
                table: "Venue",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "CAPACITY",
                table: "Venue",
                newName: "Capacity");

            migrationBuilder.RenameColumn(
                name: "ADDRESS",
                table: "Venue",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "VENUEID",
                table: "Venue",
                newName: "Venueid");

            migrationBuilder.RenameColumn(
                name: "IMAGE_URL",
                table: "Venue",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "STATUS",
                table: "Event",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "LOCATION",
                table: "Event",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "EVENT_TIME",
                table: "Event",
                newName: "EventTime");

            migrationBuilder.RenameColumn(
                name: "EVENT_NAME",
                table: "Event",
                newName: "EventName");

            migrationBuilder.RenameColumn(
                name: "EVENT_END_TIME",
                table: "Event",
                newName: "EventEndTime");

            migrationBuilder.RenameColumn(
                name: "EVENT_DESCRIPTION",
                table: "Event",
                newName: "EventDescription");

            migrationBuilder.RenameColumn(
                name: "EVENT_ID",
                table: "Event",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "BOOKINGREFERENCE",
                table: "Booking",
                newName: "Bookingreference");

            migrationBuilder.RenameColumn(
                name: "BOOKINGID",
                table: "Booking",
                newName: "Bookingid");

            migrationBuilder.RenameColumn(
                name: "VENUE_ID",
                table: "Booking",
                newName: "VenueId");

            migrationBuilder.RenameColumn(
                name: "EVENTS_ID",
                table: "Booking",
                newName: "EventsId");

            migrationBuilder.RenameColumn(
                name: "CUSTOMER_NAME",
                table: "Booking",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "CUSTOMER_CONTACT",
                table: "Booking",
                newName: "CustomerContact");

            migrationBuilder.RenameColumn(
                name: "BOOKING_STATUS",
                table: "Booking",
                newName: "BookingStatus");

            migrationBuilder.RenameColumn(
                name: "BOOKING_DATE",
                table: "Booking",
                newName: "BookingDate");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_VENUE_ID",
                table: "Booking",
                newName: "IX_Booking_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_EVENTS_ID",
                table: "Booking",
                newName: "IX_Booking_EventsId");

            migrationBuilder.AlterColumn<int>(
                name: "VenueId",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventsId",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Bookingid");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Event_EventsId",
                table: "Booking",
                column: "EventsId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Venue_VenueId",
                table: "Booking",
                column: "VenueId",
                principalTable: "Venue",
                principalColumn: "Venueid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
