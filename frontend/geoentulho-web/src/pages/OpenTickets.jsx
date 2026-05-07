import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Header from '../components/Header';
import api from '../services/api';
import '../styles/ticket.css';

export default function OpenTickets() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [tickets, setTickets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [updatingId, setUpdatingId] = useState(null);

  if (!user || user.type !== 'company') {
    navigate('/dashboard');
    return null;
  }

  useEffect(() => {
    fetchTickets();
  }, []);

  const fetchTickets = async () => {
    try {
      setLoading(true);
      const response = await api.get('/api/tickets');
      setTickets(response.data || []);
    } catch (err) {
      setError('Erro ao carregar chamados');
      console.error('Erro ao carregar chamados:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleStatusUpdate = async (ticketId, newStatus) => {
    try {
      setUpdatingId(ticketId);
      await api.put(`/api/tickets/${ticketId}/status`, { status: newStatus });
      
      // Atualizar lista local
      setTickets((prev) =>
        prev.map((ticket) =>
          ticket.id === ticketId ? { ...ticket, status: newStatus } : ticket
        )
      );
    } catch (err) {
      alert('Erro ao atualizar status: ' + (err.response?.data?.message || err.message));
    } finally {
      setUpdatingId(null);
    }
  };

  const getStatusBadge = (status) => {
    const statusMap = {
      aberto: { label: 'Aberto', color: '#F4B860' },
      aceito: { label: 'Aceito', color: '#3B9B6F' },
      em_coleta: { label: 'Em Coleta', color: '#2D7A5B' },
      concluído: { label: 'Concluído', color: '#5CB385' },
    };
    return statusMap[status] || { label: status, color: '#999' };
  };

  const getWasteTypeIcon = (wasteType) => {
    const icons = {
      construção: '🏗️',
      eletrônico: '📱',
      orgânico: '🌱',
      plástico: '♻️',
      metal: '⚙️',
      vidro: '🔍',
      papel: '📄',
      madeira: '🪵',
      outros: '📦',
    };
    return icons[wasteType] || '📦';
  };

  const getNextStatus = (currentStatus) => {
    const statusFlow = {
      aberto: 'aceito',
      aceito: 'em_coleta',
      em_coleta: 'concluído',
      concluído: null,
    };
    return statusFlow[currentStatus];
  };

  const canUpdateStatus = (status) => {
    return status !== 'concluído';
  };

  return (
    <div className="dashboard">
      <Header />
      <div className="dashboard-content">
        <div className="container-dashboard">
          <div className="ticket-list-container">
            <div className="list-header">
              <button className="btn-back" onClick={() => navigate('/dashboard')}>
                ← Voltar
              </button>
              <h1>Gerenciar Chamados</h1>
              <div className="ticket-stats">
                <span>{tickets.filter((t) => t.status === 'aberto').length} Abertos</span>
              </div>
            </div>

            {error && <div className="alert alert-error">{error}</div>}

            {loading ? (
              <div className="loading">Carregando chamados...</div>
            ) : tickets.length === 0 ? (
              <div className="empty-state">
                <div className="empty-icon">📭</div>
                <h2>Nenhum chamado disponível</h2>
                <p>Aguardando novos chamados de coleta dos clientes</p>
              </div>
            ) : (
              <div className="tickets-grid">
                {tickets.map((ticket) => {
                  const statusInfo = getStatusBadge(ticket.status);
                  const nextStatus = getNextStatus(ticket.status);

                  return (
                    <div key={ticket.id} className="ticket-card company-card">
                      <div className="ticket-header">
                        <div className="ticket-icon">
                          {getWasteTypeIcon(ticket.wasteType)}
                        </div>
                        <div className="ticket-info">
                          <h3>{ticket.title}</h3>
                          <p className="ticket-type">{ticket.wasteType}</p>
                        </div>
                        <div
                          className="ticket-status"
                          style={{ backgroundColor: statusInfo.color }}
                        >
                          {statusInfo.label}
                        </div>
                      </div>

                      <div className="ticket-details">
                        {ticket.description && (
                          <p className="description">{ticket.description}</p>
                        )}

                        <div className="details-grid">
                          <div className="detail-item">
                            <span className="label">📍 Localização</span>
                            <span className="value">
                              {ticket.address}, {ticket.city} - {ticket.state}
                            </span>
                          </div>
                          {ticket.estimatedWeight && (
                            <div className="detail-item">
                              <span className="label">⚖️ Peso Estimado</span>
                              <span className="value">{ticket.estimatedWeight} kg</span>
                            </div>
                          )}
                          {ticket.phone && (
                            <div className="detail-item">
                              <span className="label">📱 Telefone</span>
                              <span className="value">{ticket.phone}</span>
                            </div>
                          )}
                          {ticket.createdByName && (
                            <div className="detail-item">
                              <span className="label">👤 Solicitante</span>
                              <span className="value">{ticket.createdByName}</span>
                            </div>
                          )}
                        </div>
                      </div>

                      <div className="ticket-actions">
                        {canUpdateStatus(ticket.status) && nextStatus && (
                          <button
                            className="btn btn-primary"
                            onClick={() => handleStatusUpdate(ticket.id, nextStatus)}
                            disabled={updatingId === ticket.id}
                          >
                            {updatingId === ticket.id
                              ? 'Atualizando...'
                              : `Avançar para ${getStatusBadge(nextStatus).label}`}
                          </button>
                        )}
                        {ticket.status === 'concluído' && (
                          <div className="completed-badge">✓ Concluído</div>
                        )}
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
