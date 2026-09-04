namespace BookShopApi.Dtos.Auth
{
    public class CurrentUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
        public bool CanAccessAdmin { get; set; }
    }
}
