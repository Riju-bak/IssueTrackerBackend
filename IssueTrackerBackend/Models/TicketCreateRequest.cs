using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class TicketCreateRequest
{
    [MaxLength(50)]
    [Required]
    public string? Title { get; set; }
    
    [MaxLength(250)] public string Description { get; set; } = "";
        
    
    [Required]
    public Status? Status { get; set; }

    [Required]
    public Priority? Priority { get; set; }

    [DisplayName("Due Date")]
    public DateTime? DueDate { get; set; }

}