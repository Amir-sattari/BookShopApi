using BookShopApi.Models;

namespace BookShopApi.Helpers
{
    public static class BookDiscountExtensions
    {
        public static bool IsCurrentlyActive(this BookDiscount discount, DateTime? utcNow = null)
        {
            var now = utcNow ?? DateTime.UtcNow;

            if (!discount.IsActive)
                return false;

            if (discount.StartDate.HasValue && now < discount.StartDate.Value)
                return false;

            if (discount.EndDate.HasValue && now > discount.EndDate.Value)
                return false;

            return true;
        }

        public static BookDiscount? GetCurrentlyActiveDiscount(this Book book, DateTime? utcNow = null)
        {
            if (book.BookDiscounts == null || book.BookDiscounts.Count == 0)
                return null;

            return book.BookDiscounts
                .Where(d => d.IsCurrentlyActive(utcNow) && d.Percentage > 0)
                .OrderByDescending(d => d.Id)
                .FirstOrDefault();
        }
    }
}
