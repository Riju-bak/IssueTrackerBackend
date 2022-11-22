using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class BoardCreateDTO
{
    [Required]
    [MaxLength(50)]
    public string? Title { get; set; }

    [Required]
    public int? Workspace { get; set; } // the workspace ID
}