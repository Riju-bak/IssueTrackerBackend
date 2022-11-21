using IssueTrackerBackend.Data;
using IssueTrackerBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly DataContext _context;

    public TicketsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Ticket>>> GetAll()
    {
        var tickets = await _context.Tickets
            .Include(t => t.Members)
            .ToListAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ticket>> Get(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null)
        {
            return NotFound($"Ticket ID: {id} not found.");
        }
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<ActionResult<Ticket>> Create([FromBody] TicketCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ticket = new Ticket();

        ticket.Title = request.Title;
        ticket.Description = request.Description;
        ticket.Status = request.Status;
        ticket.Priority = request.Priority;
        ticket.DueDate = request.DueDate;
        ticket.Members = new List<User>();

        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        return Created("", ticket);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Ticket>> Update(int id, [FromBody] TicketUpdateRequest request)
    {
        var ticket = await _context.Tickets
            .Where(t => t.Id == id)
            .Include(t => t.Members)
            .FirstOrDefaultAsync();
        if (ticket == null)
        {
            return NotFound($"Ticket ID: {id} not found");
        }
    
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        ticket.Description = request.Description;
        ticket.Priority = request.Priority;
        ticket.Status = request.Status;
        ticket.DueDate = request.DueDate;

        List<User> members = new();
        foreach (int userId in request.Members)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("One or more member ids are invalid");
            }
            members.Add(user);
        }
        ticket.Members = members;
        await _context.SaveChangesAsync();
        return Ok(ticket);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound($"Ticket ID: {id} not found");
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}