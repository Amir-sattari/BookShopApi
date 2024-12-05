using BookShopApi.Interfaces;

namespace BookShopApi.Models
{
    public class City : IDeleteable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProvinceId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Province Province { get; set; }
    }
}
