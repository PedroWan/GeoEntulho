using GeoEntulho.API.DTOs;
using GeoEntulho.API.Data;
using GeoEntulho.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GeoEntulho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ApplicationDbContext context, ILogger<TicketsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Listar chamados (cidadão vê seus, empresa vê os abertos)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<TicketDto>>> GetTickets()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userTypeClaim = User.FindFirst("Type");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            var userType = userTypeClaim?.Value;
            var query = _context.Tickets.AsQueryable();

            // Se for cidadão, vê apenas seus chamados
            if (userType == "citizen")
            {
                query = query.Where(t => t.CreatedByUserId == userId);
            }
            // Se for empresa, vê apenas chamados abertos e que aceitou
            else if (userType == "company")
            {
                query = query.Where(t => t.Status == "aberto" || t.AssignedToUserId == userId);
            }

            var tickets = await query
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            var ticketDtos = tickets.Select(t => new TicketDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                WasteType = t.WasteType,
                Address = t.Address,
                City = t.City,
                State = t.State,
                Phone = t.Phone,
                EstimatedWeight = t.EstimatedWeight,
                Status = t.Status,
                CreatedByUserId = t.CreatedByUserId,
                CreatedByName = t.CreatedByUser?.Name,
                AssignedToUserId = t.AssignedToUserId,
                AssignedToName = t.AssignedToUser?.Name,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();

            return Ok(ticketDtos);
        }

        /// <summary>
        /// Criar novo chamado (apenas cidadão)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket([FromBody] CreateTicketDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userTypeClaim = User.FindFirst("Type");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            if (userTypeClaim?.Value != "citizen")
                return Forbid("Apenas cidadãos podem criar chamados");

            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                WasteType = dto.WasteType,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                Phone = dto.Phone,
                EstimatedWeight = dto.EstimatedWeight,
                Status = "aberto",
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Ticket created: {ticket.Id} by user {userId}");

            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, MapToDto(ticket));
        }

        /// <summary>
        /// Obter chamado por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicketById(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
                return NotFound();

            return Ok(MapToDto(ticket));
        }

        /// <summary>
        /// Atualizar status do chamado (apenas empresa)
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<ActionResult<TicketDto>> UpdateTicketStatus(int id, [FromBody] UpdateTicketStatusDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userTypeClaim = User.FindFirst("Type");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            if (userTypeClaim?.Value != "company")
                return Forbid("Apenas empresas podem atualizar status");

            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
                return NotFound();

            // Validar transição de status
            var validStatuses = new[] { "aceito", "em_coleta", "concluído" };
            if (!validStatuses.Contains(dto.Status))
                return BadRequest("Status inválido");

            // Se for 'aceito', atribuir à empresa
            if (dto.Status == "aceito")
            {
                ticket.AssignedToUserId = userId;
                ticket.AssignedToUser = await _context.Users.FindAsync(userId);
            }

            ticket.Status = dto.Status;
            ticket.UpdatedAt = DateTime.UtcNow;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Ticket {id} status updated to {dto.Status} by company {userId}");

            return Ok(MapToDto(ticket));
        }

        private TicketDto MapToDto(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                WasteType = ticket.WasteType,
                Address = ticket.Address,
                City = ticket.City,
                State = ticket.State,
                Phone = ticket.Phone,
                EstimatedWeight = ticket.EstimatedWeight,
                Status = ticket.Status,
                CreatedByUserId = ticket.CreatedByUserId,
                CreatedByName = ticket.CreatedByUser?.Name,
                AssignedToUserId = ticket.AssignedToUserId,
                AssignedToName = ticket.AssignedToUser?.Name,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }
    }
}
