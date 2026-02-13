using MediatR;
using Microsoft.EntityFrameworkCore;
using EXAM1.Data;
using EXAM1.Models.DTOs;
using EXAM1.Models.Entities;
using FluentValidation; // Tambahkan ini

namespace EXAM1.Features.BookTicket
{
    public class BookTicketHandler : IRequestHandler<BookTicketCommand, BookTicketResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<BookTicketCommand> _validator; // Tambahkan field validator

        // Update Constructor untuk menyuntikkan IValidator
        public BookTicketHandler(ApplicationDbContext context, IValidator<BookTicketCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<BookTicketResponse> Handle(BookTicketCommand request, CancellationToken cancellationToken)
        {
            // 1. Jalankan Validasi secara manual sebelum logic lainnya
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                // Melempar ValidationException agar ditangkap oleh GlobalExceptionHandler
                throw new ValidationException(validationResult.Errors);
            }

            // 2. Logic bisnis Anda tetap di bawah sini
            var ticketcodes = request.Items.Select(i => i.TicketCode).ToList();
            var tickets = await _context.Tickets
                .Include(x => x.Category)
                .Where(t => ticketcodes.Contains(t.Code))
                .ToListAsync(cancellationToken);

            foreach (var item in request.Items)
            {
                var ticket = tickets.FirstOrDefault(t => t.Code == item.TicketCode);

                if (ticket == null)
                {
                    throw new KeyNotFoundException($"Ticket code {item.TicketCode} not found.");
                }
                if (ticket.AvailableQuota == 0)
                {
                    throw new Exception($"Ticket code {item.TicketCode} is sold out.");
                }
                if (ticket.AvailableQuota < item.Quantity)
                {
                    throw new Exception($"Quantity exceeds available quota for {item.TicketCode}");
                }
                if (ticket.EventDate < DateTime.Now.Date)
                {
                    throw new Exception($"Ticket {item.TicketCode} event date has passed");
                }
            }

            // ... (sisa kode insert ke database dan mapping response tetap sama)
            var bookedItems = new BookedTicket { BookingDate = DateTime.Now };
            _context.BookedTickets.Add(bookedItems);
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var item in request.Items)
            {
                var ticket = tickets.First(t => t.Code == item.TicketCode);
                var bookedTicketItem = new BookedTicketItem
                {
                    BookedTicketId = bookedItems.Id,
                    TicketId = ticket.Id,
                    Quantity = item.Quantity,
                };
                _context.BookedTicketItems.Add(bookedTicketItem);
                ticket.AvailableQuota -= item.Quantity;
            }
            await _context.SaveChangesAsync(cancellationToken);

            var groupedCategory = tickets
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryTicketGroup
                {
                    CategoryName = g.Key,
                    Tickets = g.Select(t => new BookedTicketDetail
                    {
                        TicketCode = t.Code,
                        TicketName = t.Name,
                        Price = t.Price,
                    }).ToList(),
                    SummaryPrice = g.Sum(t => t.Price * request.Items.First(i => i.TicketCode == t.Code).Quantity)
                }).ToList();

            return new BookTicketResponse
            {
                PriceSummary = groupedCategory.Sum(c => c.SummaryPrice),
                TicketsPerCategories = groupedCategory
            };
        }
    }
}