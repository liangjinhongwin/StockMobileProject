using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMobileProject.Models;

namespace StockMobileProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserStock> UserStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserStock>()
                .HasKey(us => new { us.Email, us.Symbol });

            modelBuilder.Entity<UserStock>()
                .HasOne(us => us.ApplicationUser)
                .WithMany(au => au.UserStocks)
                .HasForeignKey(fk => fk.Email)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
