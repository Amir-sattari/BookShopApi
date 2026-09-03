namespace BookShopApi.Helpers
{
    public enum StockNotificationStatus
    {
        Created,
        AlreadyRequested,
        BookNotFound,
        BookInStock
    }

    public class StockNotificationResult
    {
        public bool IsSuccess { get; init; }
        public StockNotificationStatus Status { get; init; }
        public string Message { get; init; } = string.Empty;

        public static StockNotificationResult Created() => new()
        {
            IsSuccess = true,
            Status = StockNotificationStatus.Created,
            Message = "درخواست شما ثبت شد. وقتی کتاب موجود شود به شما اطلاع می‌دهیم."
        };

        public static StockNotificationResult AlreadyRequested() => new()
        {
            IsSuccess = false,
            Status = StockNotificationStatus.AlreadyRequested,
            Message = "درخواست شما قبلاً ثبت شده است."
        };

        public static StockNotificationResult BookNotFound() => new()
        {
            IsSuccess = false,
            Status = StockNotificationStatus.BookNotFound,
            Message = "کتاب یافت نشد."
        };

        public static StockNotificationResult BookInStock() => new()
        {
            IsSuccess = false,
            Status = StockNotificationStatus.BookInStock,
            Message = "این کتاب در حال حاضر موجود است."
        };
    }
}
