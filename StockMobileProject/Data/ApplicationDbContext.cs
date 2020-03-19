using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMobileProject.Models;

namespace StockMobileProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
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
                .HasKey(p => new { p.Email, p.Symbol });

            modelBuilder.Entity<UserStock>()
                .HasOne(p => p.ApplicationUser)
                .WithMany(p => p.Portfolio)
                .HasForeignKey(fk => fk.Email)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
