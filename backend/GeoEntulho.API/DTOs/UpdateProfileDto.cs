namespace GeoEntulho.API.DTOs
{
    public class UpdateProfileDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        
        // Para empresas
        public string? CompanyName { get; set; }
        public string? CompanyWebsite { get; set; }
    }
}
