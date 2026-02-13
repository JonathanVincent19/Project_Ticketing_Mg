using EXAM1.Models.Entities;

namespace EXAM1.Models.DTOs
{
    public class GetAvailableTicketsResponse
    {
        public DateTime EventDate { get; set; }
        public int Quota { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class BookedTicketItemDto
    {
        public int TicketId { get; set; }
        public int Quantity { get; set; }
    }

    //public class GetAvailableTicketsResponseComparer : IEqualityComparer<GetAvailableTicketsResponse>
    //{
    //    public bool Equals(GetAvailableTicketsResponse? x, GetAvailableTicketsResponse? y)
    //    {
    //        if (x is null && y is null) return true;
    //        if (x is null || y is null) return false;
    //        return x.EventDate == y.EventDate &&
    //               x.Quota == y.Quota &&
    //               x.Code == y.Code &&
    //               x.Name == y.Name &&
    //               x.Category == y.Category &&
    //               x.Price == y.Price;
    //    }
    //    public int GetHashCode(GetAvailableTicketsResponse obj)
    //    {
    //        return HashCode.Combine(obj.EventDate, obj.Quota, obj.Code, obj.Name, obj.Category, obj.Price);
    //    }
    //}
}
