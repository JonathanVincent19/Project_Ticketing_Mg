using FluentValidation;
using EXAM1.Models.DTOs;

namespace EXAM1.Features.BookTicket
{
    public class BookTicketValidator : AbstractValidator<BookTicketCommand>
    {
        public BookTicketValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Items cannot be empty.")
                .Must(x => x.Count > 0).WithMessage("At least one item is required.");

            RuleForEach(x => x.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(i => i.TicketCode)
                        .NotEmpty()
                        .WithMessage("TicketCode is required");
                    item.RuleFor(i => i.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than 0");

                });
            RuleFor(x => x.Items).NotEmpty();
        }
    }
}