using MediatR;
using EXAM1.Models.DTOs;

namespace EXAM1.Features.GetAvailableTickets
{
    public class GetAvailableTicketsQuery : IRequest<PaginatedResponse<GetAvailableTicketsResponse>>
    {
        public string? CategoryName { get; set; }      
        public string? TicketCode { get; set; }       
        public string? TicketName { get; set; }
        public decimal? Price { get; set; }
        public DateTime? EventDate { get; set; }
        
        public string? OrderBy { get; set; }
        public string? OrderState { get; set; }

        public int? Page { get; set; }
        public int? PageSize { get; set; }

    }
}
