namespace GeoEntulho.API.Models;

public class TicketUpdate
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string? OldStatus { get; set; }
    public required string NewStatus { get; set; }
    public string? Message { get; set; }
    public int UpdatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public virtual required Ticket Ticket { get; set; }
    public virtual required User UpdatedByUser { get; set; }
}
