using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class EventSearchViewModel
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
