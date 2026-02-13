using EXAM1.Features.BookTicket;
using EXAM1.Features.RevokeTicket;
using EXAM1.Features.GetAvailableTickets;
using EXAM1.Features.GetBookedTicketDetail;
using EXAM1.Features.EditBookedTicket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EXAM1.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-available-ticket")]
        public async Task<IActionResult> GetAvailableTickets([FromQuery] GetAvailableTicketsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("book-ticket")]
        public async Task<IActionResult> BookTicket([FromBody] BookTicketCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBookedTicketDetail), new { bookedTicketId = 0 }, result);
        }

        [HttpGet("get-booked-ticket/{bookedTicketId}")]
        public async Task<IActionResult> GetBookedTicketDetail(int bookedTicketId)
        {
            var query = new GetBookedTicketDetailQuery { BookedTicketId = bookedTicketId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("revoke-ticket/{bookedTicketId}/{ticketCode}/{quantity}")]
        public async Task<IActionResult> RevokeTicket(int bookedTicketId, string ticketCode, int quantity)
        {
            var command = new RevokeTicketCommand
            {
                BookedTicketId = bookedTicketId,
                TicketCode = ticketCode,
                Quantity = quantity
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("edit-booked-ticket/{bookedTicketId}")]
        public async Task<IActionResult> EditBookedTicket(int bookedTicketId, [FromBody] EditBookedTicketCommand command)
        {
            command.BookedTicketId = bookedTicketId;  
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}