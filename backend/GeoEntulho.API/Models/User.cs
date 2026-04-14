namespace GeoEntulho.API.Models;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; } // "citizen" ou "company"
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public virtual Company? Company { get; set; }
    public virtual ICollection<Ticket> TicketsAsCreator { get; set; } = new List<Ticket>();
    public virtual ICollection<Ticket> TicketsAsCompany { get; set; } = new List<Ticket>();
    public virtual ICollection<TicketUpdate> TicketUpdates { get; set; } = new List<TicketUpdate>();
}
