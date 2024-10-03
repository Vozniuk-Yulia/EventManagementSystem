using System.Net.Sockets;

namespace EventManagementSystem.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int MaxAttendees { get; set; }
        public bool IsLimitedAttendance { get; set; }
        public string? OnlineLink { get; set; }
        public int LocationID { get; set; }
        public virtual Location Location { get; set; }
        public Category Category { get; set; }
        public int CategoryID { get; set; }
        public Format Format { get; set; }
        public int FormatID { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public virtual ICollection<Ticket> Tickets { get; set; }= new List<Ticket>();
    }
}
