namespace BookShopApi.Helpers
{
    public static class BookPriceHelper
    {
        public static decimal GetEffectivePrice(decimal price, decimal discountPercentage)
        {
            if (discountPercentage <= 0)
                return price;

            var clamped = Math.Min(discountPercentage, 100m);
            return Math.Round(price * (100 - clamped) / 100m, 0, MidpointRounding.AwayFromZero);
        }

        public static decimal GetDiscountAmount(decimal amount, decimal discountPercentage)
        {
            if (discountPercentage <= 0 || amount <= 0)
                return 0;

            var clamped = Math.Min(discountPercentage, 100m);
            return Math.Round(amount * clamped / 100m, 0, MidpointRounding.AwayFromZero);
        }
    }
}
