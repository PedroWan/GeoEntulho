using System.Collections.Generic;

namespace GeoEntulho.API.Services
{
    public interface IFirebaseService
    {
        Task<Dictionary<string, object>> GetUserAsync(string userId);
        Task<string> CreateUserAsync(string email, string password, string name, string type);
        Task UpdateUserAsync(string userId, Dictionary<string, object> data);
        Task<Dictionary<string, object>> GetTicketAsync(string ticketId);
        Task<List<Dictionary<string, object>>> GetTicketsAsync(string userId, string role);
        Task<string> CreateTicketAsync(Dictionary<string, object> ticketData);
        Task UpdateTicketStatusAsync(string ticketId, string newStatus, string? assignedToUserId = null);
    }
}
