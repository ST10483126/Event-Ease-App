using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEaseApp.Data;
using EventEaseApp.Models;

namespace EventEaseApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventEaseContext _context;

        public EventsController(EventEaseContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index(
            string? searchString,
            int? eventTypeId,
            DateTime? dateFrom,
            DateTime? dateTo,
            bool? venueAvailable)
        {
                // 1. Fetch Categories for the Dropdown filter
                ViewBag.EventTypes = new SelectList(
                    await _context.EventType.AsNoTracking().OrderBy(et => et.EventTypeName).ToListAsync(),
                    "EventTypeId", "EventTypeName", eventTypeId);

                // 2. Retain values so they stay in form inputs after submission
                ViewBag.CurrentSearch = searchString;
                ViewBag.CurrentType = eventTypeId;
                ViewBag.CurrentDateFrom = dateFrom?.ToString("yyyy-MM-dd");
                ViewBag.CurrentDateTo = dateTo?.ToString("yyyy-MM-dd");
                ViewBag.CurrentAvailable = venueAvailable;

                // 3. Base Query including the EventType relation
                var events = _context.Event
                    .Include(e => e.EventType)
                    .AsQueryable();

                // Filter: Keyword search
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    events = events.Where(e =>
                        EF.Functions.Like(e.EventName, $"%{searchString}%") ||
                        (e.EventDescription != null && EF.Functions.Like(e.EventDescription, $"%{searchString}%")) ||
                        (e.Location != null && EF.Functions.Like(e.Location, $"%{searchString}%")));
                }

                // Filter: Event Type lookup
                if (eventTypeId.HasValue)
                {
                    events = events.Where(e => e.EventTypeId == eventTypeId.Value);
                }

                // Filter: Date Range (Using your model's EventTime property)
                if (dateFrom.HasValue)
                {
                    events = events.Where(e => e.EventTime >= dateFrom.Value);
                }
                if (dateTo.HasValue)
                {
                    events = events.Where(e => e.EventTime <= dateTo.Value.AddDays(1).AddTicks(-1));
                }

                // Filter: Booking Availability
                if (venueAvailable.HasValue && venueAvailable.Value)
                {
                    // Filters down to events that don't have any confirmed bookings yet
                    events = events.Where(e => !_context.Booking.Any(b => b.EventsId == e.EventId && b.BookingStatus == "Confirmed"));
                }

            return View(await events.ToListAsync());

        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewBag.EventTypes = new SelectList(
                _context.EventType.AsNoTracking().OrderBy(et => et.EventTypeName).ToList(),
                "EventTypeId", "EventTypeName");

            var @event = new Event { Status = "Scheduled" };
            return View(@event);
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDescription,EventTime,EventEndTime,Location,Status,EventTypeId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // If it got this far, something failed; re-populate the dropdown and show the form again
            ViewBag.EventTypes = new SelectList(
                _context.EventType.AsNoTracking().OrderBy(et => et.EventTypeName).ToList(),
                "EventTypeId", "EventTypeName", @event.EventTypeId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            ViewBag.EventTypes = new SelectList(
                _context.EventType.AsNoTracking().OrderBy(et => et.EventTypeName).ToList(),
                "EventTypeId", "EventTypeName", @event.EventTypeId);

            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDescription,EventTime,EventEndTime,Location,Status,EventTypeId")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            ViewBag.EventTypes = new SelectList(
                _context.EventType.AsNoTracking().OrderBy(et => et.EventTypeName).ToList(),
                "EventTypeId", "EventTypeName", @event.EventTypeId);

            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool activeBookings = await _context.Booking.AnyAsync(b => b.EventsId == id && b.BookingStatus == "Active");

            if (activeBookings)
            {
                // If there are active bookings, do not delete the event and show an error message
                ModelState.AddModelError("", "This event has active bookings and cannot be deleted.");
                return RedirectToAction(nameof(Delete), new { id });
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event != null)
            {
                _context.Event.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
    }
}
