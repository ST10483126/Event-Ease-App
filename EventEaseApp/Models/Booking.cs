using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseApp.Models;

[Table("Bookings")]
public partial class Booking
{
    [Column("BOOKINGID")]
    public int Bookingid { get; set; }

    [Column("BOOKINGREFERENCE")]
    public string Bookingreference { get; set; }

    [Column("BOOKING_DATE")]
    public DateTime? BookingDate { get; set; }

    [Column("BOOKING_STATUS")]
    public string? BookingStatus { get; set; }

    [Column("VENUE_ID")]
    public int? VenueId { get; set; }

    [Column("EVENTS_ID")]
    public int? EventsId { get; set; }

    [Column("CUSTOMER_NAME")]
    public string CustomerName { get; set; }

    [Column("CUSTOMER_CONTACT")]
    public string CustomerContact { get; set; }

    [Column("IS_AVAILABLE")]
    public bool IsAvailable { get; set; } = true;

    public virtual Event? Events { get; set; }

    public virtual Venue? Venue { get; set; }
}