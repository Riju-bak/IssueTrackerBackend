using System.ComponentModel.DataAnnotations;

namespace IssueTrackerBackend.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    public string Username { get; set; }

    public string Email { get; set; }
    
    public byte[] PasswordSalt { get; set; }

    public byte[] PasswordHash { get; set; }

    public string Role { get; set; } = "user";
}