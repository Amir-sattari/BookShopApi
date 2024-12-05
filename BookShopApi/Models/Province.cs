using BookShopApi.Interfaces;

namespace BookShopApi.Models
{
    public class Province : IDeleteable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
