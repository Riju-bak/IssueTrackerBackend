using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class BoardUpdateDTO
{
    [Required]
    [MaxLength(50)]
    public string? Title { get; set; }
}