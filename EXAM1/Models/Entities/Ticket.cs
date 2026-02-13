namespace EXAM1.Models.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public DateTime EventDate { get; set; }
        public int TotalQuota { get; set; }
        public int AvailableQuota { get; set; }

        public Category Category { get; set; } = new Category();

        public ICollection<BookedTicketItem> BookedTicketItems { get; set; } = new List<BookedTicketItem>();
    }
}
