namespace BookShopApi.Dtos.BookDiscount
{
    public class BookDiscountDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public decimal Percentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrentlyActive { get; set; }
    }
}
