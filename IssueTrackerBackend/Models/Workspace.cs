using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class Workspace
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; } = "";

    public List<User> Members { get; set; } = new();

    public List<Board> Boards { get; set; } = new();
}