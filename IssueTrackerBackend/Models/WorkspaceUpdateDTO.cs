using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class WorkspaceUpdateDTO : WorkspaceCreateDTO
{
    public List<int> Members { get; set; }
}