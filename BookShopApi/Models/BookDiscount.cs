namespace BookShopApi.Models
{
    public class BookDiscount
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        public decimal Percentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
