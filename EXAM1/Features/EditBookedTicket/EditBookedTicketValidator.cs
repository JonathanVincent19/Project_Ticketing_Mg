using FluentValidation;

namespace EXAM1.Features.EditBookedTicket
{
    public class EditBookedTicketValidator : AbstractValidator<EditBookedTicketCommand>
    {
        public EditBookedTicketValidator()
        {
            RuleFor(x => x.BookedTicketId)
                .GreaterThan(0)
                .WithMessage("BookedTicketId must be greater than 0.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Items cannot be empty");

            RuleForEach(x => x.Items)
                .ChildRules(items =>
                {
                    items.RuleFor(i => i.TicketCode)
                        .NotEmpty()
                        .WithMessage("TicketCode is required.");

                    items.RuleFor(i => i.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than 0.");
                });
        }
    }
}
