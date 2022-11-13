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
        return Ok(await _context.Tickets.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ticket>> Get(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound($"Ticket ID: {id} not found.");
        }
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<ActionResult<Ticket>> Create([FromBody] Ticket ticket)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        return Created("", ticket);
    }

    [HttpPut]
    public async Task<ActionResult<Ticket>> Update([FromBody] Ticket request)
    {
        var ticket = await _context.Tickets.FindAsync(request.Id);
        if (ticket == null)
        {
            return NotFound($"Ticket ID: {request.Id} not found");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ticket.Description = request.Description;
        ticket.Priority = request.Priority;
        ticket.Status = request.Status;
        ticket.DueDate = request.DueDate;
        ticket.AssignedTo = request.AssignedTo;
        ticket.WatchedBy = request.WatchedBy;

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