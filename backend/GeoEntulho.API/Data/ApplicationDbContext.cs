using GeoEntulho.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoEntulho.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CollectionPoint> CollectionPoints { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketUpdate> TicketUpdates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Company Configuration
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cnpj).IsRequired().HasMaxLength(20);
            entity.HasOne(e => e.User)
                .WithOne(e => e.Company)
                .HasForeignKey<Company>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CollectionPoint Configuration
        modelBuilder.Entity<CollectionPoint>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Address).IsRequired();
            entity.HasOne(e => e.Company)
                .WithMany(e => e.CollectionPoints)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Ticket Configuration
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Address).IsRequired();
            entity.Property(e => e.ResidueType).IsRequired().HasMaxLength(100);
            
            entity.HasOne(e => e.Citizen)
                .WithMany(e => e.TicketsAsCreator)
                .HasForeignKey(e => e.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Company)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasOne(e => e.CollectionPoint)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.CollectionPointId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // TicketUpdate Configuration
        modelBuilder.Entity<TicketUpdate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NewStatus).IsRequired().HasMaxLength(50);
            
            entity.HasOne(e => e.Ticket)
                .WithMany(e => e.Updates)
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.UpdatedByUser)
                .WithMany(e => e.TicketUpdates)
                .HasForeignKey(e => e.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
