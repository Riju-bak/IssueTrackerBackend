using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IssueTrackerBackend.Models;

public class Board
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Title { get; set; }

    public List<Ticket> Tickets { get; set; } = new();
}
