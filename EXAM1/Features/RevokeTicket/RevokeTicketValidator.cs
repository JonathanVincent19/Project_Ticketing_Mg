using MediatR;
using EXAM1.Models.DTOs;
using FluentValidation;

namespace EXAM1.Features.RevokeTicket
{
    public class RevokeTicketValidator : AbstractValidator<RevokeTicketCommand>
    {
        public RevokeTicketValidator()
        {
            RuleFor(x => x.BookedTicketId)
                 .GreaterThan(0)
                 .WithMessage("BookedTicketId must be greater than zero.");
            RuleFor(x => x.TicketCode)
                 .NotEmpty()
                 .WithMessage("Quantity is required.");
            RuleFor(x => x.Quantity)
                 .GreaterThan(0)
                 .WithMessage("Quantity must be greater than zero.");
        }
    }
}
