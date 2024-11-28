using BookShopApi.Dtos.Bookmark;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class BookmarkMappers
    {
        public static Bookmark ToBookmarkFromCreateDto(this CreateBookmarkDto bookmarkDto)
        {
            return new Bookmark
            {
                UserId = bookmarkDto.UserId,
                BookId = bookmarkDto.BookId,
            };
        }
    }
}
