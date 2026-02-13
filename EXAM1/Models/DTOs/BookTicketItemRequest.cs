namespace EXAM1.Models.DTOs
{
    public class BookTicketItemRequest
    {
        public string TicketCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}