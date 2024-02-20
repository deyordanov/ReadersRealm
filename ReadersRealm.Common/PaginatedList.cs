namespace ReadersRealm.Common;

public class PaginatedList<T> : List<T>
{
    public PaginatedList(List<T> items, int totalCount, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Floor(totalCount / (double)pageSize);
        AddRange(items);
    }

    public int PageIndex { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage => PageIndex > 0;

    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> Create(List<T> data, int pageIndex, int pageSize)
    {
        int totalCount = data.Count;
        List<T> items = data.Skip(pageIndex * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, totalCount, pageIndex, pageSize);
    }
}