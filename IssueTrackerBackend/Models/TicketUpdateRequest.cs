namespace IssueTrackerBackend.Models;

public class TicketUpdateRequest : TicketCreateRequest
{
    public List<int> Members { get; set; }
}