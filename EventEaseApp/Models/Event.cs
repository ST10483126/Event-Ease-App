using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseApp.Models;

[Table("Events")]
public partial class Event
{
    [Column("EVENT_ID")]
    public int EventId { get; set; }

    [Column("EVENT_NAME")]
    public string EventName { get; set; } = null!;

    [Column("EVENT_DESCRIPTION")]
    public string? EventDescription { get; set; }

    [Column("EVENT_TIME")]
    public DateTime EventTime { get; set; }

    [Column("EVENT_END_TIME")]
    public DateTime? EventEndTime { get; set; }

    [Column("LOCATION")]
    public string? Location { get; set; }

    [Column("STATUS")]
    public string? Status { get; set; }

    [Column("EVENTTYPE_ID")]
    public int? EventTypeId { get; set; }

    public virtual EventType? EventType { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}