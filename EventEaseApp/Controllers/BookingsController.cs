using EventEaseApp.Data;
using EventEaseApp.Models;
using EventEaseApp.Services;
using EventEaseApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventEaseApp.Controllers
/// <summary>
/// LINQ REFERENCE:
/// Using LINQ join syntax to combine Booking, Event, and Venue tables
/// Creates a consolidated view without exposing foreign key IDs
/// Source: Microsoft Docs - LINQ Joins (2024)
/// URL: https://learn.microsoft.com/en-us/dotnet/csharp/linq/perform-inner-joins
/// Accessed: June 2026
/// </summary>
{
    public class BookingsController : Controller
    {
        private readonly EventEaseContext _context;
        private readonly BlobService _blobService;

        public BookingsController(EventEaseContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Bookings with FILTERING using ViewModel
        public async Task<IActionResult> Index(string searchString, string customerName,
                                               DateTime? bookingDate, string bookingStatus,
                                               bool? bookingAvailable)
        {
            var query = from b in _context.Booking
                        join e in _context.Event on b.EventsId equals e.EventId
                        join v in _context.Venue on b.VenueId equals v.Venueid
                        select new BookingViewModel
                        {
                            BookingId = b.Bookingid,
                            BookingReference = b.Bookingreference,
                            BookingDate = b.BookingDate,
                            BookingStatus = b.BookingStatus,
                            CustomerName = b.CustomerName,
                            CustomerContact = b.CustomerContact,
                            EventName = e.EventName,
                            EventTime = e.EventTime,
                            EventLocation = e.Location,
                            VenueName = v.Venuename,
                            VenueAddress = v.Address,
                            VenueCapacity = v.Capacity,
                            IsAvailable = b.IsAvailable
                        };

            // Apply filters
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(b => b.BookingReference.Contains(searchString) ||
                                         b.CustomerName.Contains(searchString) ||
                                         b.EventName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(b => b.CustomerName.Contains(customerName));
            }

            if (bookingDate.HasValue)
            {
                query = query.Where(b => b.BookingDate.HasValue &&
                                         b.BookingDate.Value.Date == bookingDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(bookingStatus))
            {
                query = query.Where(b => b.BookingStatus == bookingStatus);
            }

            if (bookingAvailable.HasValue && bookingAvailable.Value)
            {
                query = query.Where(b => b.IsAvailable == true);
            }

            // Store filter values in ViewBag
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentCustomerName = customerName;
            ViewBag.CurrentBookingDate = bookingDate?.ToString("yyyy-MM-dd");
            ViewBag.CurrentBookingStatus = bookingStatus;
            ViewBag.CurrentAvailable = bookingAvailable;
            ViewBag.Statuses = new SelectList(new[] { "Pending", "Confirmed", "Cancelled", "Completed" });

            return View(await query.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Events)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.Bookingid == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewBag.VenueId = new SelectList(_context.Venue, "Venueid", "Venuename");
            ViewBag.EventsId = new SelectList(_context.Event, "EventId", "EventName");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Bookingreference,BookingDate,BookingStatus,VenueId,EventsId,CustomerName,CustomerContact,IsAvailable")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.VenueId = new SelectList(_context.Venue, "Venueid", "Venuename", booking.VenueId);
            ViewBag.EventsId = new SelectList(_context.Event, "EventId", "EventName", booking.EventsId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.VenueId = new SelectList(_context.Venue, "Venueid", "Venuename", booking.VenueId);
            ViewBag.EventsId = new SelectList(_context.Event, "EventId", "EventName", booking.EventsId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Bookingid,Bookingreference,BookingDate,BookingStatus,VenueId,EventsId,CustomerName,CustomerContact,IsAvailable")] Booking booking)
        {
            if (id != booking.Bookingid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Bookingid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.VenueId = new SelectList(_context.Venue, "Venueid", "Venuename", booking.VenueId);
            ViewBag.EventsId = new SelectList(_context.Event, "EventId", "EventName", booking.EventsId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Events)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.Bookingid == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Bookingid == id);
        }

        // POST: Bookings/ToggleAvailability/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAvailability(int id, bool isAvailable)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound(new { success = false, message = "Booking not found" });
            }

            booking.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Updated successfully" });
        }
    }
}