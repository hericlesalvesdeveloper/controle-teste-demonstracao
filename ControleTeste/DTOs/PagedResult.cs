namespace ControleTeste.DTOs;

public record PagedResult<T>(IEnumerable<T> Items, int TotalItems, int PageNumber, int PageSize)
{
}
