using MediatR;
using Microsoft.EntityFrameworkCore;
using EXAM1.Data;
using EXAM1.Models.DTOs;

namespace EXAM1.Features.GetAvailableTickets
{
    public class GetAvailableTicketsHandler : IRequestHandler<GetAvailableTicketsQuery, PaginatedResponse<GetAvailableTicketsResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetAvailableTicketsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<GetAvailableTicketsResponse>> Handle(GetAvailableTicketsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Tickets
                .Where(t => t.AvailableQuota > 0)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                query = query.Where(t => t.Category.Name.Contains(request.CategoryName));
            }
            if (!string.IsNullOrEmpty(request.TicketCode))
                query = query.Where(t => t.Code.Contains(request.TicketCode));

            if (!string.IsNullOrEmpty(request.TicketName))
                query = query.Where(t => t.Name.Contains(request.TicketName));

            if (request.Price.HasValue)
                query = query.Where(t => t.Price == request.Price);

            if (request.EventDate.HasValue)
                query = query.Where(t => t.EventDate.Date == request.EventDate.Value.Date);

            // Sorting
            var orderBy = request.OrderBy ?? "Code";  // Default: Code
            var orderState = request.OrderState?.ToLower() ?? "asc";  // Default: asc

            query = orderBy switch
            {
                "CategoryName" => orderState == "desc"
                    ? query.OrderByDescending(t => t.Category.Name)
                    : query.OrderBy(t => t.Category.Name),

                "Code" => orderState == "desc"
                    ? query.OrderByDescending(t => t.Code)
                    : query.OrderBy(t => t.Code),

                "Name" => orderState == "desc"
                    ? query.OrderByDescending(t => t.Name)
                    : query.OrderBy(t => t.Name),

                "Price" => orderState == "desc"
                    ? query.OrderByDescending(t => t.Price)
                    : query.OrderBy(t => t.Price),

                "EventDate" => orderState == "desc"
                    ? query.OrderByDescending(t => t.EventDate)
                    : query.OrderBy(t => t.EventDate),

                "AvailableQuota" => orderState == "desc"
                    ? query.OrderByDescending(t => t.AvailableQuota)
                    : query.OrderBy(t => t.AvailableQuota),

                _ => query.OrderBy(t => t.Code)  // Default jika OrderBy tidak valid
            };

            var totalCount = await query.CountAsync(cancellationToken);
            int page = request.Page ?? 1;
            int pageSize = request.PageSize ?? 10;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var data = await query
                .Select(t => new GetAvailableTicketsResponse
                {
                    Category = t.Category.Name,
                    Code = t.Code,
                    Name = t.Name,
                    EventDate = t.EventDate,
                    Price = t.Price,
                    Quota = t.AvailableQuota
                })
                .ToListAsync(cancellationToken);

            return new PaginatedResponse<GetAvailableTicketsResponse>
            {
                Data = data,
                TotalTicket = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }
    }
}
