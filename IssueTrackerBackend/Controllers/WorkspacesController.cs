using IssueTrackerBackend.Data;
using IssueTrackerBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkspacesController : ControllerBase
{
    private readonly DataContext _context;

    public WorkspacesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Workspace>>> GetAll()
    {
        var workspaces = await _context.Workspaces
            .Include(w => w.Boards)
            .Include(w => w.Members)
            .ToListAsync();
        return Ok(workspaces);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Workspace>> Get(int id)
    {
        var workspace = await _context.Workspaces
            .Include(w => w.Boards)
            .Include(w => w.Members)
            .FirstOrDefaultAsync(w => w.Id == id);
        if (workspace == null)
        {
            return NotFound($"Workspace Id: {id} not found");
        }

        return Ok(workspace);
    }

    [HttpPost]
    public async Task<ActionResult<Workspace>> Create(WorkspaceCreateDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var workspace = new Workspace();
        workspace.Title = request.Title;
        workspace.Description = request.Description;

        await _context.Workspaces.AddAsync(workspace);
        await _context.SaveChangesAsync();
        return Created("", workspace);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Workspace>> Update(int id, [FromBody] WorkspaceUpdateDTO request)
    {
        var workspace = await _context.Workspaces
            .Where(w => w.Id == id)
            .Include(w => w.Members)
            .Include(w => w.Boards)
            .FirstOrDefaultAsync();
        if (workspace == null)
        {
            return NotFound($"Workspace ID: {id} not found");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        workspace.Title = request.Title;
        workspace.Description = request.Description;
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

        workspace.Members = members;
        await _context.SaveChangesAsync();
        return Ok(workspace);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var workspace = await _context.Workspaces.FindAsync(id);
        if (workspace == null)
        {
            return NotFound($"Workspace ID: {id} not found");
        }

        _context.Workspaces.Remove(workspace);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}