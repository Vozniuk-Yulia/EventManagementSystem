namespace EventManagementSystem.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Middlename { get; set; }
        public string Bio { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
