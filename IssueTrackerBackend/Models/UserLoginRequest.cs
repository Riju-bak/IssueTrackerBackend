using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class UserLoginRequest
{
    [Required]
    public string UsernameOrEmail { get; set; }

    [Required]
    public string Password { get; set; }
}