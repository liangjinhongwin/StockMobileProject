using StockMobileProject.Data;

namespace StockMobileProject.Models
{
    public class UserStock
    {
        public string Email { get; set; }
        public string Symbol { get; set; }
        public bool IsWatched { get; set; }
        public int PurchasedCount { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}