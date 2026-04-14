namespace GeoEntulho.API.Models;

public class CollectionPoint
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double CapacityM3 { get; set; } // capacidade em m³
    public double CurrentVolumeM3 { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public virtual required Company Company { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
