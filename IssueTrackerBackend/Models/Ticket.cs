using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public enum Status
{
    Pending,
    InProgress,
    Review,
    Complete,
    Cancelled
}

public enum Priority
{
    Low,
    Medium,
    High
}

public class Ticket
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; } = "";
        
    
    [Required]
    public Status? Status { get; set; }

    [Required]
    public Priority? Priority { get; set; }

    [DisplayName("Due Date")]
    public DateTime? DueDate { get; set; }

    public List<User> Members { get; set; }//List of users the ticket has been assigned to

}