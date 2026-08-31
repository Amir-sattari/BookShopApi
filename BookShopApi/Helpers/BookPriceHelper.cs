namespace BookShopApi.Helpers
{
    public static class BookPriceHelper
    {
        public static decimal GetEffectivePrice(decimal price, int discountPercentage)
        {
            if (discountPercentage <= 0 || discountPercentage >= 100)
            {
                return price;
            }

            return Math.Round(price * (100 - discountPercentage) / 100m, 0, MidpointRounding.AwayFromZero);
        }
    }
}
