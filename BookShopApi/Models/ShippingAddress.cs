namespace BookShopApi.Models
{
    public class ShippingAddress
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;

        // Navigation Properties
        public Province Province { get; set; }
        public City City { get; set; }

    }
}
