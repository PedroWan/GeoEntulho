namespace GeoEntulho.API.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public string? Phone { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public bool IsVerified { get; set; }
        
        // Para empresas
        public string? CompanyName { get; set; }
        public string? CompanyWebsite { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
