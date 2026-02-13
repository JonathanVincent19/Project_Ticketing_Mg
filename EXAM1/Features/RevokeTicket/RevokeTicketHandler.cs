using MediatR;
using Microsoft.EntityFrameworkCore;
using EXAM1.Data;
using EXAM1.Models.DTOs;

namespace EXAM1.Features.RevokeTicket
{
    public class RevokeTicketHandler : IRequestHandler<RevokeTicketCommand, RevokeTicketResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        public RevokeTicketHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<RevokeTicketResponse> Handle(
            RevokeTicketCommand request,
            CancellationToken cancellationToken)
        {
            var bookedTicket = await _dbContext.BookedTickets
            .Include(bt => bt.BookedTicketItems)
            .ThenInclude(bti => bti.Ticket)
            .ThenInclude(t => t.Category)
            .FirstOrDefaultAsync(bt => bt.Id == request.BookedTicketId, cancellationToken);

            // Validasi BookedTicket
            if (bookedTicket == null)
            {
                throw new KeyNotFoundException($"BookedTicketId {request.BookedTicketId} not found");
            }
            var bookedItem = bookedTicket.BookedTicketItems
            .FirstOrDefault(bti => bti.Ticket.Code == request.TicketCode);

            // Item harus ada disini
            if (bookedItem == null)
            {
                throw new KeyNotFoundException($"Ticket code {request.TicketCode} not found in this booking");
            }
            // Quantity yang direvoke !> yang sudah dibook
            if (request.Quantity > bookedItem.Quantity)
            {
                throw new InvalidOperationException(
                    $"Quantity to revoke ({request.Quantity}) exceeds booked quantity ({bookedItem.Quantity})");
            }

            var ticket = bookedItem.Ticket;
            ticket.AvailableQuota += request.Quantity;

            int remainingQuantity = bookedItem.Quantity - request.Quantity;

            if (remainingQuantity == 0)
            {
                // Jika jadi 0, hapus item ini
                _dbContext.BookedTicketItems.Remove(bookedItem);
            }
            else
            {
                // Jika masih ada sisa, update quantity
                bookedItem.Quantity = remainingQuantity;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            // Reload untuk cek sisa items
            var remainingItems = await _dbContext.BookedTicketItems
                .Where(bti => bti.BookedTicketId == request.BookedTicketId)
                .CountAsync(cancellationToken);

            // Jika sudah tidak ada items, hapus BookedTicket juga
            if (remainingItems == 0)
            {
                _dbContext.BookedTickets.Remove(bookedTicket);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return new RevokeTicketResponse
            {
                TicketCode = ticket.Code,
                TicketName = ticket.Name,
                CategoryName = ticket.Category.Name,
                RemainingQuantity = remainingQuantity
            };
        }
    }
}
