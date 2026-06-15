using GeoEntulho.API.Data;
using GeoEntulho.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoEntulho.API.Services
{
    public class SqlDataService : IFirebaseService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<SqlDataService> _logger;

        public SqlDataService(ApplicationDbContext db, ILogger<SqlDataService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>> GetUserAsync(string userId)
        {
            User? user = null;
            if (int.TryParse(userId, out var id))
            {
                user = await _db.Users.FindAsync(id);
            }
            else
            {
                user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userId);
            }

            if (user == null) return null;

            return new Dictionary<string, object>
            {
                { "id", user.Id },
                { "email", user.Email },
                { "name", user.Name },
                { "type", user.Type }
            };
        }

        public async Task<string> CreateUserAsync(string email, string password, string name, string type)
        {
            var existing = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existing != null) throw new InvalidOperationException("User already exists");

            var user = new User
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Name = name,
                Type = type
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user.Id.ToString();
        }

        public async Task UpdateUserAsync(string userId, Dictionary<string, object> data)
        {
            if (!int.TryParse(userId, out var id)) return;
            var user = await _db.Users.FindAsync(id);
            if (user == null) return;

            if (data.ContainsKey("name")) user.Name = data["name"].ToString();
            if (data.ContainsKey("email")) user.Email = data["email"].ToString();

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<Dictionary<string, object>> GetTicketAsync(string ticketId)
        {
            if (!int.TryParse(ticketId, out var id)) return null;
            var ticket = await _db.Tickets.Include(t => t.Updates).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null) return null;

            return new Dictionary<string, object>
            {
                { "id", ticket.Id },
                { "title", ticket.Title },
                { "description", ticket.Description ?? "" },
                { "wasteType", ticket.WasteType },
                { "address", ticket.Address },
                { "city", ticket.City },
                { "state", ticket.State },
                { "status", ticket.Status },
                { "createdByUserId", ticket.CreatedByUserId },
                { "createdAt", ticket.CreatedAt },
            };
        }

        public async Task<List<Dictionary<string, object>>> GetTicketsAsync(string userId, string role)
        {
            List<Ticket> tickets;
            if (role == "citizen")
            {
                // userId is email here historically: try to resolve
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userId);
                if (user == null) return new List<Dictionary<string, object>>();
                tickets = await _db.Tickets.Where(t => t.CreatedByUserId == user.Id).ToListAsync();
            }
            else
            {
                tickets = await _db.Tickets.Where(t => t.Status == "aberto").ToListAsync();
            }

            return tickets.Select(t => new Dictionary<string, object>
            {
                { "id", t.Id },
                { "title", t.Title },
                { "wasteType", t.WasteType },
                { "address", t.Address },
                { "city", t.City },
                { "state", t.State },
                { "status", t.Status },
                { "createdByUserId", t.CreatedByUserId },
                { "createdAt", t.CreatedAt }
            }).ToList();
        }

        public async Task<string> CreateTicketAsync(Dictionary<string, object> ticketData)
        {
            // Resolve creator
            var createdBy = ticketData.ContainsKey("createdByUserId") ? ticketData["createdByUserId"].ToString() : null;
            User? user = null;
            if (!string.IsNullOrEmpty(createdBy))
            {
                user = await _db.Users.FirstOrDefaultAsync(u => u.Email == createdBy) ?? await _db.Users.FindAsync(int.Parse(createdBy));
            }

            var ticket = new Ticket
            {
                Title = ticketData.GetValueOrDefault("title")?.ToString() ?? "",
                Description = ticketData.GetValueOrDefault("description")?.ToString(),
                WasteType = ticketData.GetValueOrDefault("wasteType")?.ToString() ?? "",
                Address = ticketData.GetValueOrDefault("address")?.ToString() ?? "",
                City = ticketData.GetValueOrDefault("city")?.ToString() ?? "",
                State = ticketData.GetValueOrDefault("state")?.ToString() ?? "",
                Phone = ticketData.GetValueOrDefault("phone")?.ToString(),
                EstimatedWeight = ticketData.ContainsKey("estimatedWeight") ? Convert.ToDecimal(ticketData["estimatedWeight"]) : null,
                Status = ticketData.GetValueOrDefault("status")?.ToString() ?? "aberto",
                CreatedByUserId = user?.Id ?? 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Tickets.Add(ticket);
            await _db.SaveChangesAsync();

            return ticket.Id.ToString();
        }

        public async Task UpdateTicketStatusAsync(string ticketId, string newStatus, string? assignedToUserId = null)
        {
            if (!int.TryParse(ticketId, out var id)) return;
            var ticket = await _db.Tickets.FindAsync(id);
            if (ticket == null) return;

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(assignedToUserId))
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == assignedToUserId) ?? await _db.Users.FindAsync(int.Parse(assignedToUserId));
                if (user != null) ticket.AssignedToUserId = user.Id;
            }

            _db.Tickets.Update(ticket);
            await _db.SaveChangesAsync();
        }
    }
}
