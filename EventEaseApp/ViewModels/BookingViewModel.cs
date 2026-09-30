using System;

namespace EventEaseApp.ViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; }
        public DateTime? BookingDate { get; set; }
        public string BookingStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }

        // From Event table (display names, not IDs)
        public string EventName { get; set; }
        public DateTime? EventTime { get; set; }
        public string EventLocation { get; set; }

        // From Venue table (display names, not IDs)
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public int VenueCapacity { get; set; }

        // Availability
        public bool IsAvailable { get; set; }
    }
}