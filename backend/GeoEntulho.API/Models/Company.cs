namespace GeoEntulho.API.Models;

public class Company
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Cnpj { get; set; }
    public string? Address { get; set; }
    public double ServiceAreaRadiusKm { get; set; } = 10.0;
    public string Status { get; set; } = "active"; // "active", "inactive"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public virtual required User User { get; set; }
    public virtual ICollection<CollectionPoint> CollectionPoints { get; set; } = new List<CollectionPoint>();
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
