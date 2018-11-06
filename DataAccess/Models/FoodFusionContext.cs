using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models
{
    public class FoodFusionContext : DbContext
    {
        public FoodFusionContext(DbContextOptions<FoodFusionContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantMap> RestaurantMaps { get; set; }
        public DbSet<RestaurantTable> RestaurantTables { get; set; }
        public DbSet<RestaurantEmployee> RestaurantEmployees { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservedTable> ReservedTables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConfigurationManager.GetSection("ConnectionStrings:FoodFusion").ToString();
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<ReservedTable>()
                .HasOne(reservedTable => reservedTable.Table)
                .WithMany(table => table.ReservedTables)
                .HasForeignKey(reservedTable => reservedTable.RestaurantTableId)
                .HasPrincipalKey(table => table.Id)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RestaurantEmployee>()
                .HasOne(restaurantEmployee => restaurantEmployee.User)
                .WithMany(user => user.RestaurantsEmployee)
                .HasForeignKey(restaurantEmployee => restaurantEmployee.UserId)
                .HasPrincipalKey(user => user.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
