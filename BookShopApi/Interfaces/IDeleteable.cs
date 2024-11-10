namespace BookShopApi.Interfaces
{
    public interface IDeleteable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
