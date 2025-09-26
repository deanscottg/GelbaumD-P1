using Microsoft.EntityFrameworkCore;
using Barbershop.Models;

namespace Barbershop.Data;

public class BarbershopDbContext : DbContext
{

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Barber> Barbers { get; set; }

    public BarbershopDbContext( DbContextOptions<BarbershopDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Appointment>()
        .Property(a => a.HaircutType)
        .HasConversion<string>();
    }

}