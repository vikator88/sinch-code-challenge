namespace DevexpApiSdk.Common
{
    public interface IPagedResult<T>
    {
        int PageSize { get; }
        int CurrentPage { get; }
        IReadOnlyList<T> Items { get; }
    }
}
