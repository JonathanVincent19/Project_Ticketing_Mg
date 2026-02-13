using MediatR;
using EXAM1.Models.DTOs;

namespace EXAM1.Features.BookTicket
{
    public class BookTicketCommand : IRequest<BookTicketResponse>
    {
        public List<BookTicketItemRequest> Items { get; set; } = new List<BookTicketItemRequest>();
    }
}