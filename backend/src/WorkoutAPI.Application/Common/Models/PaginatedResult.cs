namespace WorkoutAPI.Application.Common.Models;

public class PaginatedResult<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public string[] Errors { get; }

    private PaginatedResult(bool isSuccess, IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount, string[] errors)
    {
        IsSuccess = isSuccess;
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        Errors = errors;
    }

    public static PaginatedResult<T> Success(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
        => new(true, items, pageNumber, pageSize, totalCount, Array.Empty<string>());

    public static PaginatedResult<T> Failure(params string[] errors)
        => new(false, new List<T>().AsReadOnly(), 0, 0, 0, errors);
}


