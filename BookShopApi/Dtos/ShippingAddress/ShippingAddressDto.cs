namespace BookShopApi.Dtos.ShippingAddress
{
    public class ShippingAddressDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
    }
}
