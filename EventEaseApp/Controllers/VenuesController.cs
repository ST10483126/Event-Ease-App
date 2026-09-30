using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEaseApp.Data;
using EventEaseApp.Models;
using EventEaseApp.Services;

namespace EventEaseApp.Controllers
{
    public class VenuesController : Controller
    {
        private readonly EventEaseContext _context;
        private readonly BlobService _blobService;

        public VenuesController(EventEaseContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Venues with FILTERING (Date filters removed - not logical for venues)
        public async Task<IActionResult> Index(string searchString, string location, int? minCapacity, bool? venueAvailable)
        {
            var query = _context.Venue.AsQueryable();

            // Filter by search string (name or address)
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(v => v.Venuename.Contains(searchString) ||
                                         v.Address.Contains(searchString) ||
                                         v.Location.Contains(searchString));
            }

            // Filter by location
            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(v => v.Location.Contains(location) || v.Address.Contains(location));
            }

            // Filter by minimum capacity
            if (minCapacity.HasValue)
            {
                query = query.Where(v => v.Capacity >= minCapacity.Value);
            }

            // Filter by availability status
            if (venueAvailable.HasValue && venueAvailable.Value)
            {
                query = query.Where(v => v.IsAvailable == true);
            }

            // Store filter values in ViewBag for the view
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentLocation = location;
            ViewBag.CurrentMinCapacity = minCapacity;
            ViewBag.CurrentAvailable = venueAvailable;

            return View(await query.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.Venueid == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Venueid,Venuename,Address,Capacity,Location,IsAvailable")] Venue venue, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("ImageUrl", "Please upload an image for the venue.");
                return View(venue);
            }

            venue.ImageUrl = await _blobService.UploadAsync(imageFile);

            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Venueid,Venuename,Address,Capacity,Location,IsAvailable")] Venue venue, IFormFile? imageFile)
        {
            if (id != venue.Venueid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingVenue = await _context.Venue.AsNoTracking().FirstOrDefaultAsync(v => v.Venueid == id);

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(existingVenue.ImageUrl))
                        {
                            await _blobService.DeleteAsync(existingVenue.ImageUrl);
                        }
                        venue.ImageUrl = await _blobService.UploadAsync(imageFile);
                    }
                    else
                    {
                        venue.ImageUrl = existingVenue.ImageUrl;
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.Venueid))
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
            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.Venueid == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool activeBookings = await _context.Booking.AnyAsync(b => b.VenueId == id);

            if (activeBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete this venue because it is associated with active bookings.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue != null)
            {
                if (!string.IsNullOrEmpty(venue.ImageUrl))
                {
                    await _blobService.DeleteAsync(venue.ImageUrl);
                }
                _context.Venue.Remove(venue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.Venueid == id);
        }
    }
}