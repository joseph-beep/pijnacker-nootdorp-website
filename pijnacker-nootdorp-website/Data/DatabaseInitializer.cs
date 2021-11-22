using System;
using System.Linq;

public static class DatabaseInitializer
{
    public static void Initialize(DatabaseContext context)
    {
        context.Database.EnsureCreated();

        // Look for any students.
        if (context.Houses.Any())
        {
            // DB has been seeded
            return;
        }

        //public DbSet<House> Houses { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }
        //public DbSet<User> Users { get; set; }

        context.Houses.Add(new House
        {
            Id = 0,
            Address = "Koningin Julianalaan",
            Price = 100,
            Description = "Een leuke huis met mooie uitzicht.",
            Access = House.Accessibility.Car | House.Accessibility.PublicTransport,
            BuildYear = 1945,
            OutdoorArea = 10.5f,
            IndoorArea = 250f,
            Rooms = ""
        });

        context.SaveChanges();


        context.Users.Add(new User
        {
            Id = 0,
            FirstName = "Joseph",
            LastName = "Thomas",
            Description = "",
            Age = 17
        });

        context.SaveChanges();
    }
}