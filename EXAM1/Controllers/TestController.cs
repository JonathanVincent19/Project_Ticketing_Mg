using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EXAM1.Data;

namespace EXAM1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("test-categories")]
        public async Task<IActionResult> TestCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return Ok(new
                {
                    message = "Database connection successful!",
                    count = categories.Count,
                    data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Database connection failed!",
                    error = ex.Message
                });
            }
        }

        [HttpGet("test-tickets")]
        public async Task<IActionResult> TestTickets()
        {
            try
            {
                var tickets = await _context.Tickets
                    .Include(t => t.Category)
                    .Take(5)
                    .ToListAsync();

                return Ok(new
                {
                    message = "Success!",
                    count = tickets.Count,
                    data = tickets.Select(t => new
                    {
                        t.Code,
                        t.Name,
                        CategoryName = t.Category.Name,
                        t.Price,
                        t.EventDate,
                        t.AvailableQuota
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed!",
                    error = ex.Message
                });
            }
        }
    }
}