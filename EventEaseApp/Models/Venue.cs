using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseApp.Models;

[Table("Venue")]
public partial class Venue
{
    [Column("VENUEID")]
    public int Venueid { get; set; }

    [Column("VENUENAME")]
    public string Venuename { get; set; } = null!;

    [Column("ADDRESS")]
    public string Address { get; set; } = null!;

    [Column("CAPACITY")]
    public int Capacity { get; set; }

    [Column("LOCATION")]
    public string Location { get; set; } = null!;

    [Column("IMAGE_URL")]
    public string? ImageUrl { get; set; }

    [Column("IS_AVAILABLE")]
    public bool IsAvailable { get; set; } = true;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}