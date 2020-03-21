using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMobileProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime StartDate { get; set; }
        public decimal Cash { get; set; }
        public string Performance { get; set; }

        // Child
        public virtual ICollection<UserStock> UserStocks { get; set; }
    }
}