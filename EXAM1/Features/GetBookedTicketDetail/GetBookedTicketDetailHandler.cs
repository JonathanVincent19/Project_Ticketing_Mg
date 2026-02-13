using MediatR;
using Microsoft.EntityFrameworkCore;
using EXAM1.Data;
using EXAM1.Models.DTOs;

namespace EXAM1.Features.GetBookedTicketDetail
{
    public class GetBookedTicketDetailHandler : IRequestHandler<GetBookedTicketDetailQuery, List<BookedTicketCategoryGroup>>
    {
        private readonly ApplicationDbContext _context;

        public GetBookedTicketDetailHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookedTicketCategoryGroup>> Handle(
            GetBookedTicketDetailQuery request,
            CancellationToken cancellationToken)
        {
            // Query BookedTicket dengan semua relasi
            var bookedTicket = await _context.BookedTickets
                .Include(bt => bt.BookedTicketItems)
                    .ThenInclude(bti => bti.Ticket)
                        .ThenInclude(t => t.Category)
                .FirstOrDefaultAsync(bt => bt.Id == request.BookedTicketId, cancellationToken);

            // Validasi: BookedTicket harus ada
            if (bookedTicket == null)
            {
                throw new KeyNotFoundException($"BookedTicketId {request.BookedTicketId} not found");
            }

            // Group by Category dan build response
            var result = bookedTicket.BookedTicketItems
                .GroupBy(bti => bti.Ticket.Category.Name)
                .Select(g => new BookedTicketCategoryGroup
                {
                    QtyPerCategory = g.Sum(bti => bti.Quantity),  // ✅ Total quantity per kategori
                    CategoryName = g.Key,
                    Tickets = g.Select(bti => new BookedTicketItemDetail
                    {
                        TicketCode = bti.Ticket.Code,
                        TicketName = bti.Ticket.Name,
                        EventDate = bti.Ticket.EventDate.ToString("dd-MM-yyyy HH:mm")  // ✅ Format khusus
                    }).ToList()
                })
                .ToList();

            return result;
        }
    }
}