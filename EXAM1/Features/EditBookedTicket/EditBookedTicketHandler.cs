using MediatR;
using Microsoft.EntityFrameworkCore;
using EXAM1.Data;
using EXAM1.Models.DTOs;
using EXAM1.Models.Entities;

namespace EXAM1.Features.EditBookedTicket
{
    public class EditBookedTicketHandler : IRequestHandler<EditBookedTicketCommand, List<EditBookedTicketResponse>>
    {
        private readonly ApplicationDbContext _context;
        public EditBookedTicketHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<EditBookedTicketResponse>> Handle(
            EditBookedTicketCommand request,
            CancellationToken cancellationToken)
        {
            var bookedTicket = await _context.BookedTickets
                .Include(bt => bt.BookedTicketItems)
                    .ThenInclude(bti => bti.Ticket)
                        .ThenInclude(t => t.Category)
                .FirstOrDefaultAsync(bt => bt.Id == request.BookedTicketId, cancellationToken);

            if (bookedTicket == null)
            {
                throw new KeyNotFoundException($"BookedTicketId {request.BookedTicketId} not found");
            }

            var responses = new List<EditBookedTicketResponse>();

            
            foreach (var itemRequest in request.Items)
            {
                
                var bookedItem = bookedTicket.BookedTicketItems
                    .FirstOrDefault(bti => bti.Ticket.Code == itemRequest.TicketCode);

               
                if (bookedItem == null)
                {
                    throw new KeyNotFoundException(
                        $"Ticket code {itemRequest.TicketCode} not found in BookedTicketId {request.BookedTicketId}");
                }

                var ticket = bookedItem.Ticket;
                int oldQuantity = bookedItem.Quantity;
                int newQuantity = itemRequest.Quantity;

                
                int difference = newQuantity - oldQuantity;

                
                if (difference > 0)
                {
                    
                    if (ticket.AvailableQuota < difference)
                    {
                        throw new InvalidOperationException(
                            $"Not enough quota for {ticket.Code}. Available: {ticket.AvailableQuota}, Needed: {difference}");
                    }
                    ticket.AvailableQuota -= difference;
                }
                else if (difference < 0)
                {
                    
                    ticket.AvailableQuota += Math.Abs(difference);
                }

                bookedItem.Quantity = newQuantity;

                responses.Add(new EditBookedTicketResponse
                {
                    TicketCode = ticket.Code,
                    TicketName = ticket.Name,
                    CategoryName = ticket.Category.Name,
                    Quantity = newQuantity.ToString()
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            return responses;
        }
    }
}
