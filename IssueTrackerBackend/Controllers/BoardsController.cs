using IssueTrackerBackend.Data;
using IssueTrackerBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BoardsController : ControllerBase
{
    private readonly DataContext _context;

    public BoardsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Board>>> GetAll()
    {
        var boards = await _context.Boards
            .Include(b => b.Tickets)
            .ToListAsync();
        return Ok(boards);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Board>> Get(int id)
    {
        var board = await _context.Boards
            .Include(b => b.Tickets)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (board == null)
        {
            return NotFound($"Board ID: {id} not found.");
        }
        return Ok(board);
    }

    [HttpPost]
    public async Task<ActionResult<Board>> Create([FromBody] BoardCreateDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int? workspaceId = request.Workspace;
        var workspace = await _context.Workspaces.FirstOrDefaultAsync(w => w.Id == workspaceId);
        if (workspace == null)
        {
            return NotFound($"Workspace ID: {workspaceId} not found.");
        }
        var board = new Board();
        board.Title = request.Title;
        workspace.Boards.Add(board);
        await _context.SaveChangesAsync();
        return Created("", board);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Board>> Update(int id, [FromBody] BoardUpdateDTO request)
    {
        var board = await _context.Boards.FindAsync(id);
        if (board == null)
        {
            return NotFound($"Board ID: {id} not found.");
        }

        board.Title = request.Title;
        await _context.SaveChangesAsync();
        return Ok(board);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var board = await _context.Boards
            .Include(b => b.Tickets)
            .Where(b => b.Id == id).
            FirstOrDefaultAsync();
        if (board == null)
        {
            return NotFound($"Board ID: {id} not found");
        }
        _context.Boards.Remove(board);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}