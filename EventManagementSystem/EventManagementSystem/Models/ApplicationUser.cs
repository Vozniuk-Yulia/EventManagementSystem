using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Middlename { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
