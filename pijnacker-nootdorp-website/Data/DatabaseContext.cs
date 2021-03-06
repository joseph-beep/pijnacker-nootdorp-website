using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<HouseSlide> HouseSlides { get; set; }
    public DbSet<House> Houses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HouseSlide>().ToTable("house-slides");
        modelBuilder.Entity<House>().ToTable("houses");
        modelBuilder.Entity<Order>().ToTable("orders");
        modelBuilder.Entity<OrderItem>().ToTable("order-items");
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Contact>().ToTable("contacts");
    }
}