using Microsoft.AspNetCore.Identity;

namespace BookShopApi.Helpers
{
    public static class UserNameHelper
    {
        public static string Combine(string firstName, string lastName)
            => $"{firstName.Trim()} {lastName.Trim()}";

        public static (string FirstName, string LastName) Split(string? fullName)
        {
            var value = (fullName ?? string.Empty).Trim();
            var space = value.IndexOf(' ');
            if (space < 0)
                return (value, string.Empty);

            return (value[..space], value[(space + 1)..].Trim());
        }

        public static string ToIdentityError(IdentityResult result)
        {
            if (result.Errors.Any(error => error.Code == "DuplicateUserName"))
                return "کاربری با این نام قبلاً ثبت شده است.";

            if (result.Errors.Any(error => error.Code == "InvalidUserName"))
                return "نام واردشده معتبر نیست.";

            return string.Join(" ", result.Errors.Select(error => error.Description));
        }
    }
}
