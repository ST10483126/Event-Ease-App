using System;
using System.Collections.Generic;
using EventEaseApp.Models;

namespace EventEaseApp.ViewModels
{
    public class DashboardViewModel
    {
        // Count Statistics
        public int TotalVenues { get; set; }
        public int TotalEvents { get; set; }
        public int TotalBookings { get; set; }
        public int PendingBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int CancelledBookings { get; set; }

        // Available Venues (IsAvailable = true)
        public int AvailableVenues { get; set; }

        // Active Bookings (IsAvailable = true)
        public int ActiveBookings { get; set; }

        // Upcoming Events (next 30 days)
        public List<Event> UpcomingEvents { get; set; }

        // Recent Bookings (last 7 days)
        public List<Booking> RecentBookings { get; set; }

        // Popular Venues (most bookings)
        public List<VenueBookingCount> PopularVenues { get; set; }

        // Monthly Booking Trend (last 6 months)
        public List<MonthlyBookingCount> MonthlyTrend { get; set; }
    }

    // Helper class for Popular Venues
    public class VenueBookingCount
    {
        public string VenueName { get; set; }
        public int BookingCount { get; set; }
    }

    // Helper class for Monthly Trend
    public class MonthlyBookingCount
    {
        public string MonthName { get; set; }
        public int Count { get; set; }
    }
}