using IssueTrackerBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBackend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Ticket>? Tickets { get; set; }
    public DbSet<User>? Users { get; set; }
    
    public DbSet<Board>? Boards { get; set; }
    
    public DbSet<Workspace>? Workspaces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Workspace>()
            .HasMany(e => e.Boards)
            .WithOne(e => e.Workspace)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        modelBuilder.Entity<Board>()
            .HasMany(e => e.Tickets)
            .WithOne(e => e.Board)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}