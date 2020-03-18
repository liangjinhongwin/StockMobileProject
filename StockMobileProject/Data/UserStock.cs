namespace StockMobileProject.Data
{
    public class UserStock
    {
        public string Email { get; set; }
        public string Symbol { get; set; }
        public bool IsWatched { get; set; }
        public int PurchasedCount { get; set; }
    }
}