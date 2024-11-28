namespace BookShopApi.Dtos.Bookmark
{
    public class CreateBookmarkDto
    {
        public string UserId { get; set; } = string.Empty;
        public int BookId { get; set; }
    }
}
