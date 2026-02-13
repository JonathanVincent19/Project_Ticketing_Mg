namespace EXAM1.Models.Entities
{
    public class BookedTicketItem
    {
        public int Id { get; set; }
        public int BookedTicketId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
        public BookedTicket BookedTicket { get; set; } = null!;
        public Ticket Ticket { get; set; } = null!;
    }
}
