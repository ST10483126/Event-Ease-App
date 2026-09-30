using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EventEaseApp.Models;

namespace EventEaseApp.Data
{
    public class EventEaseContext : DbContext
    {
        public EventEaseContext(DbContextOptions<EventEaseContext> options)
            : base(options)
        {
        }

        public DbSet<EventEaseApp.Models.Venue> Venue { get; set; } = default!;
        public DbSet<EventEaseApp.Models.Event> Event { get; set; } = default!;
        public DbSet<EventEaseApp.Models.Booking> Booking { get; set; } = default!;
        public DbSet<EventEaseApp.Models.EventType> EventType { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.HasKey(e => e.EventTypeId);
                entity.Property(e => e.EventTypeName).IsRequired().HasMaxLength(100);

                entity.HasData(
                    new EventType { EventTypeId = 1, EventTypeName = "Conference" },
                    new EventType { EventTypeId = 2, EventTypeName = "Wedding" },
                    new EventType { EventTypeId = 3, EventTypeName = "Concert" },
                    new EventType { EventTypeId = 4, EventTypeName = "Corporate" },
                    new EventType { EventTypeId = 5, EventTypeName = "Birthday" },
                    new EventType { EventTypeId = 6, EventTypeName = "Exhibition" },
                    new EventType { EventTypeId = 7, EventTypeName = "Sports" },
                    new EventType { EventTypeId = 8, EventTypeName = "Festival" },
                    new EventType { EventTypeId = 9, EventTypeName = "Anniversary" },
                    new EventType { EventTypeId = 10, EventTypeName = "Other" }
                );
            });
            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(et => et.Events)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
