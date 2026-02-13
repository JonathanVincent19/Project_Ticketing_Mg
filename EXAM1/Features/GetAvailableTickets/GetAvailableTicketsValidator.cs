using FluentValidation;
using EXAM1.Features.GetAvailableTickets;

namespace EXAM1.Features.GetAvailableTickets
{
    public class GetAvailableTicketsValidator : AbstractValidator<GetAvailableTicketsQuery>
    {
        public GetAvailableTicketsValidator()
        {
            RuleFor(x => x.OrderState)
                .Must(state => string.IsNullOrEmpty(state) ||
                    state.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                    state.Equals("desc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("OrderState harus 'asc' atau 'desc'.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Price.HasValue)
                .WithMessage("Price tidak boleh kurang dari 0");
        }
    }
}
