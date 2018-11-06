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

            modelBuilder.Entity<MenuItem>()
                .HasOne(menuItem => menuItem.Menu)
                .WithMany(menu => menu.Items)
                .HasForeignKey(menuItem => menuItem.MenuId)
                .HasPrincipalKey(menu => menu.Id);
            
            modelBuilder.Entity<RestaurantTable>()
                .HasOne(table => table.Map)
                .WithMany(map => map.Tables)
                .HasForeignKey(table => table.RestaurantMapId)
                .HasPrincipalKey(map => map.Id);

            modelBuilder.Entity<RestaurantMap>()
                .HasOne(map => map.Restaurant)
                .WithOne(restaurant => restaurant.Map)
                .HasForeignKey<Restaurant>(restaurant => restaurant.RestaurantMapId)
                .HasPrincipalKey<RestaurantMap>(map => map.Id);

            modelBuilder.Entity<ReservedTable>()
                .HasOne(reservedTable => reservedTable.Table)
                .WithMany(table => table.ReservedTables)
                .HasForeignKey(reservedTable => reservedTable.RestaurantTableId)
                .HasPrincipalKey(table => table.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReservedTable>()
                .HasOne(reservedTable => reservedTable.Reservation)
                .WithMany(reservation => reservation.ReservedTables)
                .HasForeignKey(reservedTable => reservedTable.ReservationId)
                .HasPrincipalKey(reservation => reservation.Id);

            modelBuilder.Entity<Reservation>()
                .HasOne(reservation => reservation.Restaurant)
                .WithMany(restaurant => restaurant.Reservations)
                .HasForeignKey(reservation => reservation.RestaurantId)
                .HasPrincipalKey(restaurant => restaurant.Id);

            modelBuilder.Entity<Restaurant>()
                .HasOne(restaurant => restaurant.Manager)
                .WithMany(manager => manager.ManagedRestaurants)
                .HasForeignKey(restaurant => restaurant.ManagerId)
                .HasPrincipalKey(manager => manager.Id);

            modelBuilder.Entity<RestaurantEmployee>()
                .HasOne(restaurantEmployee => restaurantEmployee.Restaurant)
                .WithMany(restaurant => restaurant.RestaurantEmployee)
                .HasForeignKey(restaurantEmployee => restaurantEmployee.RestaurantId)
                .HasPrincipalKey(restaurant => restaurant.Id);

            modelBuilder.Entity<RestaurantEmployee>()
                .HasOne(restaurantEmployee => restaurantEmployee.User)
                .WithMany(user => user.RestaurantsEmployee)
                .HasForeignKey(restaurantEmployee => restaurantEmployee.UserId)
                .HasPrincipalKey(user => user.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
