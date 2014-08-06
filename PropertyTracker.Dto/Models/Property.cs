namespace PropertyTracker.Dto.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public double SquareFeet { get; set; }

        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }
}
