using Google.Cloud.Firestore;
using Google.Cloud.Firestore.Admin;
using Firebase.Auth;
using System.Security.Claims;

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

    public class FirebaseService : IFirebaseService
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly FirebaseAuthProvider _authProvider;
        private readonly ILogger<FirebaseService> _logger;

        public FirebaseService(ILogger<FirebaseService> logger)
        {
            _logger = logger;
            
            // Inicializar Firestore
            var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID") 
                ?? throw new InvalidOperationException("FIREBASE_PROJECT_ID não configurado");
            
            _firestoreDb = FirestoreDb.Create(projectId);
            
            // Inicializar Firebase Auth
            var apiKey = Environment.GetEnvironmentVariable("FIREBASE_API_KEY")
                ?? throw new InvalidOperationException("FIREBASE_API_KEY não configurado");
            
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
        }

        public async Task<Dictionary<string, object>> GetUserAsync(string userId)
        {
            try
            {
                var docSnapshot = await _firestoreDb.Collection("users").Document(userId).GetSnapshotAsync();
                
                if (docSnapshot.Exists)
                {
                    return docSnapshot.ToDictionary();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter usuário {userId}: {ex.Message}");
                throw;
            }
        }

        public async Task<string> CreateUserAsync(string email, string password, string name, string type)
        {
            try
            {
                // Criar usuário no Firebase Auth
                var authResult = await _authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                var uid = authResult.LocalId;

                // Armazenar dados do usuário no Firestore
                var userData = new Dictionary<string, object>
                {
                    { "email", email },
                    { "name", name },
                    { "type", type },
                    { "createdAt", Timestamp.Now },
                    { "uid", uid }
                };

                await _firestoreDb.Collection("users").Document(uid).SetAsync(userData);
                
                _logger.LogInformation($"Usuário criado com sucesso: {uid}");
                return uid;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar usuário: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateUserAsync(string userId, Dictionary<string, object> data)
        {
            try
            {
                await _firestoreDb.Collection("users").Document(userId).UpdateAsync(data);
                _logger.LogInformation($"Usuário {userId} atualizado");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar usuário {userId}: {ex.Message}");
                throw;
            }
        }

        public async Task<Dictionary<string, object>> GetTicketAsync(string ticketId)
        {
            try
            {
                var docSnapshot = await _firestoreDb.Collection("tickets").Document(ticketId).GetSnapshotAsync();
                
                if (docSnapshot.Exists)
                {
                    return docSnapshot.ToDictionary();
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter ticket {ticketId}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetTicketsAsync(string userId, string role)
        {
            try
            {
                Query query = _firestoreDb.Collection("tickets");

                if (role == "citizen")
                {
                    query = query.WhereEqualTo("createdByUserId", userId);
                }
                else if (role == "company")
                {
                    query = query.WhereIn("status", new[] { "aberto", "aceito", "em_coleta", "concluído" });
                }

                var snapshot = await query.GetSnapshotAsync();
                var tickets = new List<Dictionary<string, object>>();

                foreach (var doc in snapshot.Documents)
                {
                    tickets.Add(doc.ToDictionary());
                }

                return tickets;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter tickets para {userId}: {ex.Message}");
                throw;
            }
        }

        public async Task<string> CreateTicketAsync(Dictionary<string, object> ticketData)
        {
            try
            {
                ticketData["createdAt"] = Timestamp.Now;
                ticketData["status"] = "aberto";

                var docRef = await _firestoreDb.Collection("tickets").AddAsync(ticketData);
                
                _logger.LogInformation($"Ticket criado com sucesso: {docRef.Id}");
                return docRef.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar ticket: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateTicketStatusAsync(string ticketId, string newStatus, string? assignedToUserId = null)
        {
            try
            {
                var updateData = new Dictionary<string, object>
                {
                    { "status", newStatus },
                    { "updatedAt", Timestamp.Now }
                };

                if (!string.IsNullOrEmpty(assignedToUserId))
                {
                    updateData["assignedToUserId"] = assignedToUserId;
                }

                await _firestoreDb.Collection("tickets").Document(ticketId).UpdateAsync(updateData);
                
                _logger.LogInformation($"Status do ticket {ticketId} atualizado para {newStatus}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar status do ticket {ticketId}: {ex.Message}");
                throw;
            }
        }
    }
}
