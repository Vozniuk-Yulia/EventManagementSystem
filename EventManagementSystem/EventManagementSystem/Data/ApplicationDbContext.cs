using EventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace EventManagementSystem.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Location)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .Property(e => e.Category)
                .HasConversion<string>();

            modelBuilder.Entity<Event>()
                .Property(e => e.Format)
                .HasConversion<string>();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Events)
                .WithMany(e => e.Users)
                .UsingEntity(j => j.ToTable("UserEvents"));

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Speakers)
                .WithMany(s => s.Events)
                .UsingEntity(j => j.ToTable("EventSpeakers"));

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ticket>()
               .Property(t => t.Price)
               .HasColumnType("decimal(10, 2)");
        }
    }
}
