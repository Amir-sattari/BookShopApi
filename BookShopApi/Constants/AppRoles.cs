namespace BookShopApi.Constants
{
    public static class AppRoles
    {
        public const string Manager = "Manager";
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Staff = $"{Manager},{Admin}";

        public static readonly string[] All = [Manager, Admin, User];

        public static bool IsStaff(string? role) =>
            role == Manager || role == Admin;

        public static bool IsValid(string? role) =>
            All.Contains(role);
    }
}
