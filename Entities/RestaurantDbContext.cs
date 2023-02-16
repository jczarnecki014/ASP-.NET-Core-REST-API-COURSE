using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Entities
{
    public class RestaurantDbContext:DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options):base(options)
        {

        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Address { get; set; }
        public  DbSet<Dish> Dishes{ get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
            .Property(u=> u.Name)
            .IsRequired();

            modelBuilder.Entity<User>()
            .Property(u=>u.Email)
            .IsRequired();


            modelBuilder.Entity<Restaurant>()
            .Property(u => u.name)
            .IsRequired()
            .HasMaxLength(50);

            modelBuilder.Entity<Dish>()
            .Property(u=>u.Name)
            .IsRequired();

            modelBuilder.Entity<Address>()
            .Property(u=>u.City)
            .HasMaxLength(50);

            modelBuilder.Entity<Address>()
            .Property(u=>u.Street)
            .HasMaxLength(80);
        }
    }
}
