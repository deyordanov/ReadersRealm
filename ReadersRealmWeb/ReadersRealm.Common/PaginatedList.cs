namespace ReadersRealm.Common;

public class PaginatedList<T> : List<T>
{
    public PaginatedList(List<T> items, int pageIndex, int pageSize)
    {
        this.PageIndex = pageIndex;
        this.TotalPages = (int)Math.Ceiling(items.Count / (double)pageSize);
        this.AddRange(items);
    }

    public int PageIndex { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage => this.PageIndex > 0;

    public bool HasNextPage => this.PageIndex < this.TotalPages;

    public static PaginatedList<T> Create(List<T> data, int pageIndex, int pageSize)
    {
        List<T> items = data.Skip(pageIndex * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, pageIndex, pageSize);
    }
}