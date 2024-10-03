using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementSystem.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
