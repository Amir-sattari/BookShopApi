namespace BookShopApi.Helpers
{
    public static class OTPHelper
    {
        public static string GenerateOTP()
        {
            var random = new Random();
            var OTPCode = random.Next(100000, 999999).ToString();
            return OTPCode;
        }

        public static bool VerifyOTP(string otp, string providedOtp)
        {
            return providedOtp == otp;
        }
    }
}
