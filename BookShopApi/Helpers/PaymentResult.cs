namespace BookShopApi.Helpers
{
    public class PaymentResult
    {
        public bool IsSuccessful { get; init; }
        public string? ReferenceId { get; init; }
        public string? ErrorMessage { get; init; }

        public static PaymentResult Success(string referenceId) => new()
        {
            IsSuccessful = true,
            ReferenceId = referenceId
        };

        public static PaymentResult Fail(string errorMessage) => new()
        {
            IsSuccessful = false,
            ErrorMessage = errorMessage
        };
    }
}
