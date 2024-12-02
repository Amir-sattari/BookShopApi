namespace BookShopApi.Dtos.ShoppingCart
{
    public class ShoppingCartDto
    {
        public string UserId { get; set; } = string.Empty;
        public int BookId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
