namespace BookShopApi.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Duration { get; set; }
        public string DurationUnit { get; set; } = string.Empty;
    }
}
