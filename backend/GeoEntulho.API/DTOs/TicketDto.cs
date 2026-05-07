namespace GeoEntulho.API.DTOs
{
    public class CreateTicketDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string WasteType { get; set; } // "construção", "eletrônico", "orgânico", etc
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public string? Phone { get; set; }
        public decimal? EstimatedWeight { get; set; } // em kg
    }

    public class TicketDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string WasteType { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public string? Phone { get; set; }
        public decimal? EstimatedWeight { get; set; }
        public required string Status { get; set; } // "aberto", "aceito", "em_coleta", "concluído"
        public int CreatedByUserId { get; set; }
        public string? CreatedByName { get; set; }
        public int? AssignedToUserId { get; set; }
        public string? AssignedToName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateTicketStatusDto
    {
        public required string Status { get; set; } // "aceito", "em_coleta", "concluído"
    }
}
