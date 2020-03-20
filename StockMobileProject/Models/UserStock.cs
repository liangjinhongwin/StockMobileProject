using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMobileProject.Models
{
    public class UserStock
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public bool IsWatched { get; set; }
        public int PurchasedCount { get; set; }

        // Parent
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
