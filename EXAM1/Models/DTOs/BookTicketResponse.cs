namespace EXAM1.Models.DTOs
{
    public class BookTicketResponse
    {
        public decimal PriceSummary { get; set; }
        public List<CategoryTicketGroup> TicketsPerCategories { get; set; } = new List<CategoryTicketGroup>();
    }
}