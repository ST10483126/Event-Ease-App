using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseApp.Models
{
    [Table("EventTypes")]
    public partial class EventType
    {
        [Column("EVENTTYPE_ID")]
        public int EventTypeId { get; set; }

        [Column("EVENTTYPE_NAME")]
        public string EventTypeName { get; set; } = null!;

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}