namespace PropertyTracker.Dto.Models
{
    public interface IPaginatedList
    {        
        int CurrentPage { get; set; }
        int TotalPages { get; set; }
        int TotalItems { get; set; }
    }
}
