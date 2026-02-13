namespace EXAM1.Models.Entities
{
    public class BookedTicket
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }

        public ICollection<BookedTicketItem> BookedTicketItems { get; set; } = new List<BookedTicketItem>();
    }
}
