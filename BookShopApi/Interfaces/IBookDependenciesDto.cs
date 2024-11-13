namespace BookShopApi.Interfaces
{
    public interface IBookDependenciesDto
    {
        List<int> CategoryIds { get; set; }
        int PublicationId { get; set; }
        int BookSizeId { get; set; }
        int CoverTypeId { get; set; }
    }
}
