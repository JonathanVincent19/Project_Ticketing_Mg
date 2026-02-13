namespace EXAM1.Models.DTOs
{
    public class BookedTicketCategoryGroup
    {
        public int QtyPerCategory { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public List<BookedTicketItemDetail> Tickets { get; set; } = new List<BookedTicketItemDetail>();
    }
}
