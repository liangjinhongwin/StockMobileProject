using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMobileProject.Models;

namespace StockMobileProject.Data
{
<<<<<<< HEAD
    public class ApplicationUser : IdentityUser
=======
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
>>>>>>> branch-kimo
    {
        public DateTime StartDate { get; set; }
        public Decimal Cash { get; set; }
        public string Performance { get; set; }
        public ICollection<UserStock> UserStocks { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserStock> UserStocks { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserStock> UserStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStock>()
                .HasKey(c => new { c.Email, c.Symbol });
            modelBuilder.Entity<UserStock>()
                .HasOne(p => p.ApplicationUser)
                .WithMany(p => p.UserStocks)
                .HasForeignKey(fk => fk.Email)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);

<<<<<<< HEAD
=======
            modelBuilder.Entity<ApplicationUser>()
                .HasKey(au => au.Id);

            modelBuilder.Entity<UserStock>()
                .HasKey(us => new { us.Id, us.Symbol });

            modelBuilder.Entity<UserStock>()
                .HasOne(us => us.ApplicationUser)
                .WithMany(au => au.UserStocks)
                .HasForeignKey(fk => fk.Id)
                .OnDelete(DeleteBehavior.Restrict);
>>>>>>> branch-kimo
        }
    }
}
