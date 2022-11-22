using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class WorkspaceUpdateDTO
{
    
    [Required]
    [MaxLength(20)]
    public string? Title { get; set; }

    [MaxLength(250)]
    public string Description { get; set; } = "";
    [Required]
    public List<int>? Members { get; set; }
}