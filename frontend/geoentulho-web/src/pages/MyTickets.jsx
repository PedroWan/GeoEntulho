import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Header from '../components/Header';
import api from '../services/api';
import '../styles/ticket.css';

export default function MyTickets() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [tickets, setTickets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  if (!user || user.type !== 'citizen') {
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
              <h1>Meus Chamados</h1>
              <button className="btn btn-primary" onClick={() => navigate('/create-ticket')}>
                + Novo Chamado
              </button>
            </div>

            {error && <div className="alert alert-error">{error}</div>}

            {loading ? (
              <div className="loading">Carregando chamados...</div>
            ) : tickets.length === 0 ? (
              <div className="empty-state">
                <div className="empty-icon">📋</div>
                <h2>Nenhum chamado criado</h2>
                <p>Comece criando um novo chamado para solicitar coleta de resíduos</p>
                <button className="btn btn-primary" onClick={() => navigate('/create-ticket')}>
                  Criar Primeiro Chamado
                </button>
              </div>
            ) : (
              <div className="tickets-grid">
                {tickets.map((ticket) => {
                  const statusInfo = getStatusBadge(ticket.status);
                  return (
                    <div key={ticket.id} className="ticket-card">
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
                          {ticket.createdAt && (
                            <div className="detail-item">
                              <span className="label">📅 Data</span>
                              <span className="value">
                                {new Date(ticket.createdAt).toLocaleDateString('pt-BR')}
                              </span>
                            </div>
                          )}
                        </div>
                      </div>

                      {ticket.assignedToName && (
                        <div className="ticket-assigned">
                          <span className="label">Atribuído a:</span>
                          <span className="value">{ticket.assignedToName}</span>
                        </div>
                      )}
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
