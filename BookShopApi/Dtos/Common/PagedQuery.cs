namespace BookShopApi.Dtos.Common
{
    public class PagedQuery
    {
        public const int MaxPageSize = 100;

        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? Search { get; set; }
        public string? SortKey { get; set; }
        public string? SortDirection { get; set; }

        public bool IsPaged => Page is > 0 && PageSize is > 0;

        public void Normalize()
        {
            Search = string.IsNullOrWhiteSpace(Search) ? null : Search.Trim();
            SortKey = string.IsNullOrWhiteSpace(SortKey) ? null : SortKey.Trim();
            SortDirection = string.IsNullOrWhiteSpace(SortDirection) ? null : SortDirection.Trim().ToLowerInvariant();

            if (!IsPaged)
            {
                Page = null;
                PageSize = null;
                return;
            }

            if (PageSize > MaxPageSize)
                PageSize = MaxPageSize;
        }
    }
}
