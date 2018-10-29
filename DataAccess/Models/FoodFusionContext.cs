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
        public DbSet<ReservationTable> ReservationTables { get; set; }

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

            modelBuilder.Entity<MenuItem>()
                .HasOne(menuItem => menuItem.Menu)
                .WithMany(menu => menu.Items)
                .HasForeignKey(menuItem => menuItem.MenuId)
                .HasPrincipalKey(menu => menu.Id);

            modelBuilder.Entity<Menu>()
                .HasOne(menu => menu.Restaurant)
                .WithOne(restaurant => restaurant.Menu)
                .HasForeignKey<Restaurant>(restaurant => restaurant.MenuId)
                .HasPrincipalKey<Menu>(menu => menu.Id);
            
            modelBuilder.Entity<RestaurantTable>()
                .HasOne(table => table.RestaurantMap)
                .WithMany(map => map.Tables)
                .HasForeignKey(table => table.RestaurantMapId)
                .HasPrincipalKey(map => map.Id);

            modelBuilder.Entity<RestaurantMap>()
                .HasOne(map => map.Restaurant)
                .WithOne(restaurant => restaurant.Map)
                .HasForeignKey<Restaurant>(restaurant => restaurant.RestaurantMapId)
                .HasPrincipalKey<RestaurantMap>(map => map.Id);


            modelBuilder.Entity<ReservationTable>()
                .HasOne(reservationTable => reservationTable.RestaurantTable)
                .WithMany(table => table.ReservationTables)
                .HasForeignKey(reservationTable => reservationTable.RestaurantTableId)
                .HasPrincipalKey(table => table.Id);
        }
    }
}
