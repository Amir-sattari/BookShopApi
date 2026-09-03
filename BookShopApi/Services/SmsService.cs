using BookShopApi.Interfaces;

namespace BookShopApi.Services
{
    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;

        public SmsService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string phoneNumber, string message)
        {
            // TODO: integrate real SMS provider once available.
            // Example of what this will look like:
            // var response = await _httpClient.PostAsync(smsProviderUrl, ...);
            // if (!response.IsSuccessStatusCode) throw new SmsSendException(...);

            _logger.LogInformation("SMS stub (not sent). To: {PhoneNumber}. Message: {Message}", phoneNumber, message);
            return Task.CompletedTask;
        }
    }
}
