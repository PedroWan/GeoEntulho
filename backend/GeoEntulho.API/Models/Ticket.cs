using System.ComponentModel.DataAnnotations.Schema;

namespace GeoEntulho.API.Models;

public class Ticket
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string WasteType { get; set; } // "construção", "eletrônico", "orgânico", etc
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public string? Phone { get; set; }
    public decimal? EstimatedWeight { get; set; } // em kg
    public required string Status { get; set; } // "aberto", "aceito", "em_coleta", "concluído"
    
    // Relacionamentos principais
    public int CreatedByUserId { get; set; }
    public virtual User? CreatedByUser { get; set; }
    
    public int? AssignedToUserId { get; set; }
    public virtual User? AssignedToUser { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Colunas legadas mantidas para compatibilidade (nullable para migrations)
    [Obsolete("Use CreatedByUserId instead")]
    public int? CitizenId { get; set; }
    
    [Obsolete("Use AssignedToUserId instead")]
    public int? CompanyId { get; set; }
    
    public int? CollectionPointId { get; set; }
    
    [Obsolete("Use Status instead")]
    public string? Type { get; set; }
    
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime? ScheduledDate { get; set; }
    
    [Obsolete("Use WasteType instead")]
    public string? ResidueType { get; set; }
    
    public double? VolumeM3 { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Relacionamentos legados marcados como NotMapped para não criar relacionamentos
    [NotMapped]
    public virtual User? Citizen { get; set; }
    
    [NotMapped]
    public virtual Company? Company { get; set; }
    
    [NotMapped]
    public virtual CollectionPoint? CollectionPoint { get; set; }
    
    public virtual ICollection<TicketUpdate> Updates { get; set; } = new List<TicketUpdate>();
}
