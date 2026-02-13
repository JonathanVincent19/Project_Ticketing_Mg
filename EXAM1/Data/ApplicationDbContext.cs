using EXAM1.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EXAM1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<BookedTicket> BookedTickets { get; set; } = null!;
        public DbSet<BookedTicketItem> BookedTicketItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Tickets");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Tickets)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<BookedTicket>(entity =>
            {
                entity.ToTable("BookedTickets");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BookingDate).IsRequired();
            });

            
            modelBuilder.Entity<BookedTicketItem>(entity =>
            {
                entity.ToTable("BookedTicketItems");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.BookedTicket)
                    .WithMany(bt => bt.BookedTicketItems)
                    .HasForeignKey(e => e.BookedTicketId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Ticket)
                    .WithMany(t => t.BookedTicketItems)
                    .HasForeignKey(e => e.TicketId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
