using GeoEntulho.API.DTOs;
using GeoEntulho.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Google.Cloud.Firestore;

namespace GeoEntulho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(IFirebaseService firebaseService, ILogger<TicketsController> logger)
        {
            _firebaseService = firebaseService;
            _logger = logger;
        }

        /// <summary>
        /// Listar chamados (cidadão vê seus, empresa vê os abertos)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetTickets()
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email);
                var typeClaim = User.FindFirst("Type");

                if (emailClaim == null || typeClaim == null)
                    return Unauthorized(new { message = "Token inválido" });

                var tickets = await _firebaseService.GetTicketsAsync(emailClaim.Value, typeClaim.Value);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting tickets: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao obter tickets" });
            }
        }

        /// <summary>
        /// Criar novo chamado (apenas cidadão)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> CreateTicket([FromBody] CreateTicketDto dto)
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email);
                var typeClaim = User.FindFirst("Type");
                var nameClaim = User.FindFirst(ClaimTypes.Name);

                if (emailClaim == null || typeClaim == null)
                    return Unauthorized(new { message = "Token inválido" });

                if (typeClaim.Value != "citizen")
                    return Forbid("Apenas cidadãos podem criar chamados");

                var ticketData = new Dictionary<string, object>
                {
                    { "title", dto.Title ?? "" },
                    { "description", dto.Description ?? "" },
                    { "wasteType", dto.WasteType ?? "" },
                    { "address", dto.Address ?? "" },
                    { "city", dto.City ?? "" },
                    { "state", dto.State ?? "" },
                    { "phone", dto.Phone ?? "" },
                    { "estimatedWeight", dto.EstimatedWeight ?? 0 },
                    { "status", "aberto" },
                    { "createdByUserId", emailClaim.Value },
                    { "createdByName", nameClaim?.Value ?? "" },
                    { "createdAt", Timestamp.Now },
                    { "updatedAt", Timestamp.Now }
                };

                var ticketId = await _firebaseService.CreateTicketAsync(ticketData);

                _logger.LogInformation($"Ticket created: {ticketId} by user {emailClaim.Value}");

                return CreatedAtAction(nameof(GetTicketById), new { id = ticketId }, new { id = ticketId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating ticket: {ex.Message}");
                return StatusCode(500, new { message = $"Erro ao criar ticket: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obter chamado por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTicketById(string id)
        {
            try
            {
                var ticket = await _firebaseService.GetTicketAsync(id);

                if (ticket == null)
                    return NotFound(new { message = "Ticket não encontrado" });

                return Ok(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting ticket {id}: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao obter ticket" });
            }
        }

        /// <summary>
        /// Atualizar status do chamado (apenas empresa)
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<ActionResult<object>> UpdateTicketStatus(string id, [FromBody] UpdateTicketStatusDto dto)
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email);
                var typeClaim = User.FindFirst("Type");
                var nameClaim = User.FindFirst(ClaimTypes.Name);

                if (emailClaim == null || typeClaim == null)
                    return Unauthorized(new { message = "Token inválido" });

                if (typeClaim.Value != "company")
                    return Forbid("Apenas empresas podem atualizar status");

                // Validar transições de status
                var validStatuses = new[] { "aberto", "aceito", "em_coleta", "concluído" };
                if (!validStatuses.Contains(dto.Status))
                    return BadRequest(new { message = "Status inválido" });

                // Se for aceitar, atribuir à empresa
                string? assignedUserId = dto.Status == "aceito" ? emailClaim.Value : null;

                await _firebaseService.UpdateTicketStatusAsync(id, dto.Status, assignedUserId);

                _logger.LogInformation($"Ticket {id} status updated to {dto.Status} by {emailClaim.Value}");

                return Ok(new { success = true, message = "Status atualizado" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating ticket {id}: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao atualizar ticket" });
            }
        }
    }
}
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
