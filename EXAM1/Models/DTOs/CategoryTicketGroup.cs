namespace EXAM1.Models.DTOs
{
    public class CategoryTicketGroup
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal SummaryPrice { get; set; }
        public List<BookedTicketDetail> Tickets { get; set; } = new List<BookedTicketDetail>();
    }
}