using BookShopApi.Data;
using BookShopApi.Dtos.ShoppingCart;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Services
{
    public class PriceCalculationService : IPriceCalculationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICouponRepository _couponRepo;

        public PriceCalculationService(ApplicationDbContext context, ICouponRepository couponRepository)
        {
            _context = context;
            _couponRepo = couponRepository;
        }

        public decimal GetBookEffectivePrice(Book book)
        {
            var percentage = book.GetCurrentlyActiveDiscount()?.Percentage ?? 0;
            return BookPriceHelper.GetEffectivePrice(book.Price, percentage);
        }

        public bool BookHasActiveDiscount(Book book)
        {
            return book.GetCurrentlyActiveDiscount() != null;
        }

        public CartPriceResult CalculateCartTotal(IEnumerable<CartPriceItem> items, Coupon? coupon)
        {
            var applicableBookIds = coupon?.ApplicableBooks?.Select(b => b.Id).ToHashSet() ?? [];
            var restrictToList = coupon != null && applicableBookIds.Count > 0;

            decimal subtotal = 0;
            decimal bookDiscountTotal = 0;
            decimal couponDiscountTotal = 0;
            var lines = new List<CartLinePriceResult>();

            foreach (var item in items)
            {
                var book = item.Book;
                var quantity = item.Quantity;
                var lineSubtotal = book.Price * quantity;
                subtotal += lineSubtotal;

                var hasBookDiscount = BookHasActiveDiscount(book);
                var unitEffectivePrice = GetBookEffectivePrice(book);
                var lineAfterBookDiscount = unitEffectivePrice * quantity;
                var bookDiscountAmount = lineSubtotal - lineAfterBookDiscount;
                bookDiscountTotal += bookDiscountAmount;

                decimal couponDiscountAmount = 0;
                var discountType = hasBookDiscount ? "BookDiscount" : "None";

                if (coupon != null && !hasBookDiscount)
                {
                    var couponApplies = !restrictToList || applicableBookIds.Contains(book.Id);
                    if (couponApplies && coupon.Percentage > 0)
                    {
                        couponDiscountAmount = BookPriceHelper.GetDiscountAmount(lineAfterBookDiscount, coupon.Percentage);
                        discountType = "Coupon";
                    }
                }

                couponDiscountTotal += couponDiscountAmount;

                lines.Add(new CartLinePriceResult
                {
                    BookId = book.Id,
                    BookTitle = book.Title,
                    Quantity = quantity,
                    UnitPrice = book.Price,
                    UnitEffectivePrice = unitEffectivePrice,
                    LineSubtotal = lineSubtotal,
                    BookDiscountAmount = bookDiscountAmount,
                    CouponDiscountAmount = couponDiscountAmount,
                    LineTotal = lineAfterBookDiscount - couponDiscountAmount,
                    DiscountType = discountType
                });
            }

            var couponHadNoEffect = coupon != null && couponDiscountTotal == 0;

            return new CartPriceResult
            {
                Subtotal = subtotal,
                BookDiscountAmount = bookDiscountTotal,
                CouponDiscountAmount = couponDiscountTotal,
                FinalTotal = subtotal - bookDiscountTotal - couponDiscountTotal,
                CouponCode = coupon?.Code,
                CouponHadNoEffect = couponHadNoEffect,
                Message = couponHadNoEffect
                    ? "Coupon is valid but did not apply to any items."
                    : null,
                Items = lines
            };
        }

        public async Task<CartPriceResult> CalculateCartTotalAsync(
            IEnumerable<CartItemDto> items,
            string? couponCode,
            string userId)
        {
            var itemList = items.ToList();
            if (itemList.Count == 0)
                throw new ArgumentException("Cart is empty.");

            var bookIds = itemList.Select(i => i.BookId).Distinct().ToList();
            var books = await _context.Books
                .Include(b => b.BookDiscounts)
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            var bookMap = books.ToDictionary(b => b.Id);
            var missing = bookIds.Where(id => !bookMap.ContainsKey(id)).ToList();
            if (missing.Count > 0)
                throw new ArgumentException($"Books not found: {string.Join(", ", missing)}");

            var priceItems = itemList.Select(item => new CartPriceItem
            {
                Book = bookMap[item.BookId],
                Quantity = item.Quantity
            });

            Coupon? coupon = null;
            if (!string.IsNullOrWhiteSpace(couponCode))
            {
                var validation = await _couponRepo.ValidateForUseAsync(couponCode, userId);
                if (!validation.IsValid)
                    throw new CouponValidationException(validation.ErrorMessage ?? "Coupon is not valid.");

                coupon = validation.Coupon;
            }

            return CalculateCartTotal(priceItems, coupon);
        }
    }
}
