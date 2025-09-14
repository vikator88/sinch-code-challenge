namespace DevexpApiSdk.Common
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }

        public PagedResult(IReadOnlyList<T> items, int currentPage, int pageSize)
        {
            Items = items;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
