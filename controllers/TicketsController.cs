using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketApi;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    //private static readonly List<Ticket> _tickets = new List<Ticket>
    //{
    //    new Ticket("Damaged product"),
    //    new Ticket("Wrong product delivered")
    //};
    private readonly TicketDbContext _context;

    public TicketsController(TicketDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _context.Tickets.ToListAsync();
        return Ok(tickets);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketDto dto)
    {
        var ticket = new Ticket(dto.Title);
        _context.Add(ticket);
        await _context.SaveChangesAsync();
        return Ok(ticket);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Ticket? found = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

        if(found == null)
        {
            return NotFound();
        }

        return Ok(found);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateTicketStatusDto dto)
    {
        Ticket? found = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

        if(found == null)
        {
            return NotFound();
        }
        else
        {
            found.Status = dto.status;
            await _context.SaveChangesAsync();
            return Ok(found);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        Ticket? found = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

        if(found == null)
        {
            return NotFound();
        }
        else
        {
            _context.Remove(found);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}