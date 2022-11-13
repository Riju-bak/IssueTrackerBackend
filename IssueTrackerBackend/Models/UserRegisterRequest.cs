using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class UserRegisterRequest
{
    [Required, MinLength(4), MaxLength(30)]
    public string Username { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }

    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}