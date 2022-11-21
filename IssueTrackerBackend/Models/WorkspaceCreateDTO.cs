using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class WorkspaceCreateDTO
{
    [Required]
    [MaxLength(20)]
    public string? Title { get; set; }

    [MaxLength(250)] public string Description { get; set; } = "";
}