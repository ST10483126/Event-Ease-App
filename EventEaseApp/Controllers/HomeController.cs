using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEaseApp.Data;
using EventEaseApp.Models;
using EventEaseApp.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

/*
 * EVENTEASE APPLICATION - TECHNICAL REFERENCES
 * ============================================
 * 
 * 1. BOOTSTRAP 5 (2024) - UI Components
 *    https://getbootstrap.com/docs/5.0/
 * 
 * 2. ENTITY FRAMEWORK CORE (2024) - Database ORM
 *    https://learn.microsoft.com/en-us/ef/core/
 * 
 * 3. AZURE BLOB STORAGE (2024) - Image Storage
 *    https://learn.microsoft.com/en-us/azure/storage/blobs/
 * 
 * 4. MDN WEB DOCS (2024) - CSS & JavaScript
 *    https://developer.mozilla.org/
 * 
 * 5. ASP.NET CORE MVC (2024) - Web Framework
 *    https://learn.microsoft.com/en-us/aspnet/core/mvc/
 * 
 * All external code implementations follow best practices
 * as documented in the official sources above.
 */

namespace EventEaseApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventEaseContext _context;

        public HomeController(EventEaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var dashboard = new DashboardViewModel();

            // Basic Counts
            dashboard.TotalVenues = await _context.Venue.CountAsync();
            dashboard.TotalEvents = await _context.Event.CountAsync();
            dashboard.TotalBookings = await _context.Booking.CountAsync();

            // Booking Status Counts
            dashboard.PendingBookings = await _context.Booking.CountAsync(b => b.BookingStatus == "Pending");
            dashboard.ConfirmedBookings = await _context.Booking.CountAsync(b => b.BookingStatus == "Confirmed");
            dashboard.CompletedBookings = await _context.Booking.CountAsync(b => b.BookingStatus == "Completed");
            dashboard.CancelledBookings = await _context.Booking.CountAsync(b => b.BookingStatus == "Cancelled");

            // Available Venues
            dashboard.AvailableVenues = await _context.Venue.CountAsync(v => v.IsAvailable == true);

            // Active Bookings
            dashboard.ActiveBookings = await _context.Booking.CountAsync(b => b.IsAvailable == true);

            // Upcoming Events (next 30 days)
            var thirtyDaysFromNow = DateTime.Now.AddDays(30);
            dashboard.UpcomingEvents = await _context.Event
                .Where(e => e.EventTime >= DateTime.Now && e.EventTime <= thirtyDaysFromNow)
                .OrderBy(e => e.EventTime)
                .Take(10)
                .ToListAsync();

            // Recent Bookings (last 7 days)
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            dashboard.RecentBookings = await _context.Booking
                .Include(b => b.Venue)
                .Include(b => b.Events)
                .Where(b => b.BookingDate >= sevenDaysAgo)
                .OrderByDescending(b => b.BookingDate)
                .Take(10)
                .ToListAsync();

            // Popular Venues (top 5 by booking count)
            dashboard.PopularVenues = await _context.Booking
                .GroupBy(b => b.VenueId)
                .Select(g => new VenueBookingCount
                {
                    VenueName = _context.Venue.Where(v => v.Venueid == g.Key).Select(v => v.Venuename).FirstOrDefault() ?? "Unknown",
                    BookingCount = g.Count()
                })
                .OrderByDescending(v => v.BookingCount)
                .Take(5)
                .ToListAsync();

            // Monthly Booking Trend (last 6 months)
            var monthlyData = new List<MonthlyBookingCount>();
            for (int i = 5; i >= 0; i--)
            {
                var month = DateTime.Now.AddMonths(-i);
                var monthStart = new DateTime(month.Year, month.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var count = await _context.Booking
                    .Where(b => b.BookingDate >= monthStart && b.BookingDate <= monthEnd)
                    .CountAsync();

                monthlyData.Add(new MonthlyBookingCount
                {
                    MonthName = month.ToString("MMM yyyy"),
                    Count = count
                });
            }
            dashboard.MonthlyTrend = monthlyData;

            return View(dashboard);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}