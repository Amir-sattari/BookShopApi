namespace BookShopApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string BookTitle { get; set; } = string.Empty;

        public Order Order { get; set; }
        public Book Book { get; set; }
    }
}
