using MediatR;
using EXAM1.Models.DTOs;

namespace EXAM1.Features.EditBookedTicket
{
    public class EditBookedTicketCommand : IRequest<List<EditBookedTicketResponse>>
    {
        public int BookedTicketId { get; set; }
        public List<EditBookedTicketItemRequest> Items { get; set; } = new List<EditBookedTicketItemRequest>();
    }
}

