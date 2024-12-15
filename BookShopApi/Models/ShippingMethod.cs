namespace BookShopApi.Models
{
    public class ShippingMethod
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
