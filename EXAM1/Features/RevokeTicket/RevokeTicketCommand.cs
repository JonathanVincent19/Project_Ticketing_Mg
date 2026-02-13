using MediatR;
using EXAM1.Models.DTOs;

namespace EXAM1.Features.RevokeTicket
{
    public class RevokeTicketCommand : IRequest<RevokeTicketResponse>
    {
        public int? BookedTicketId { get; set; }
        public string? TicketCode { get; set; }
        public int Quantity { get; set; }
    }
}
