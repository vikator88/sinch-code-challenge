namespace DevexpApiSdk.Common
{
    public interface IPagedResult<T>
    {
        public int PageSize { get; }
        public int CurrentPage { get; }
        public IReadOnlyList<T> Items { get; }
    }
}
