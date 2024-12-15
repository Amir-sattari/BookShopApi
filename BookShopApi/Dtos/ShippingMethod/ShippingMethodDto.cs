namespace BookShopApi.Dtos.ShippingMethod
{
    public class ShippingMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
