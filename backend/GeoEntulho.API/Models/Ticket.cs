namespace GeoEntulho.API.Models;

public class Ticket
{
    public int Id { get; set; }
    public int CitizenId { get; set; }
    public int? CompanyId { get; set; }
    public int? CollectionPointId { get; set; }
    public required string Type { get; set; } // "drop_off" ou "pickup"
    public required string Status { get; set; } // "pending", "accepted", "in_progress", "completed", "cancelled"
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public required string ResidueType { get; set; } // madeira, alvenaria, metal, misto, etc
    public double VolumeM3 { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    // Relacionamentos
    public virtual required User Citizen { get; set; }
    public virtual Company? Company { get; set; }
    public virtual CollectionPoint? CollectionPoint { get; set; }
    public virtual ICollection<TicketUpdate> Updates { get; set; } = new List<TicketUpdate>();
}
