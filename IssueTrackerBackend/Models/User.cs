using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IssueTrackerBackend.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    public string Username { get; set; }

    public string Email { get; set; }
    
    [JsonIgnore]
    public byte[] PasswordSalt { get; set; }

    [JsonIgnore]
    public byte[] PasswordHash { get; set; }

    [JsonIgnore]
    public string Role { get; set; } = "user";

    // without JsonIgnore data annotation we'll get circular object error
    [JsonIgnore]
    public List<Ticket> Tickets { get; set; } //List of all tickets that have been assigned to the user

    [JsonIgnore]
    public List<Workspace> Workspaces { get; set; } //List of all workspaces the user is a part of
}