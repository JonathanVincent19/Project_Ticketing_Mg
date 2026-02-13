namespace EXAM1.Models.DTOs
{
    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalTicket { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
