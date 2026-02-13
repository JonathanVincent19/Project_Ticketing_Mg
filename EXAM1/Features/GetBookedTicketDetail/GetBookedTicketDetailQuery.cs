using MediatR;
using EXAM1.Models.DTOs;



namespace EXAM1.Features.GetBookedTicketDetail
{
    public class GetBookedTicketDetailQuery : IRequest<List<BookedTicketCategoryGroup>>
    {
        public int BookedTicketId { get; set; }
    }
}
