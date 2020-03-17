﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StockMobileProject.Data
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime StartDate { get; set; }
        public Decimal Cash { get; set; }
        public string Performance { get; set; }
        //public ICollection<UserStock> Portfolio { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public DbSet<UserStock> UserStocks { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserStock>()
            //    .HasKey(c => new { c.Email, c.Symbol });
            base.OnModelCreating(modelBuilder);

        }
    }
}
