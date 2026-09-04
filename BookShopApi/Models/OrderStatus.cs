namespace BookShopApi.Models
{
    public enum OrderStatus
    {
        PendingPayment = 0,
        Paid = 1,
        PaymentFailed = 2,
        Cancelled = 3
    }
}
