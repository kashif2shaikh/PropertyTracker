namespace PropertyTracker.Dto.Models
{
    public interface IPaginatedList
    {        
        int CurrentPage { get; set; }
        int TotalPages { get; set; }
        int PageSize { get; set; }
        int TotalItems { get; set; }
    }
}
