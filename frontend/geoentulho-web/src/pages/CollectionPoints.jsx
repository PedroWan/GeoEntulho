import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Header from '../components/Header';
import '../styles/ticket.css';

export default function CollectionPoints() {
  const { user } = useAuth();
  const navigate = useNavigate();

  if (!user) {
    navigate('/login');
    return null;
  }

  const collectionPoints = [
    {
      id: 1,
      name: 'Ponto de Coleta Centro',
      address: 'Avenida Brasil, 1000',
      city: 'Belo Horizonte',
      state: 'MG',
      phone: '(31) 3333-4444',
      hours: 'Seg-Sex: 08:00 - 17:00',
      types: ['construção', 'plástico', 'metal'],
    },
    {
      id: 2,
      name: 'Ponto de Coleta Savassi',
      address: 'Rua da Bahia, 2500',
      city: 'Belo Horizonte',
      state: 'MG',
      phone: '(31) 3333-5555',
      hours: 'Seg-Sab: 09:00 - 18:00',
      types: ['eletrônico', 'vidro', 'papel'],
    },
    {
      id: 3,
      name: 'Ponto de Coleta Pampulha',
      address: 'Avenida Liberdade, 3000',
      city: 'Belo Horizonte',
      state: 'MG',
      phone: '(31) 3333-6666',
      hours: 'Ter-Dom: 08:30 - 16:30',
      types: ['orgânico', 'madeira', 'outros'],
    },
  ];

  const getTypeEmoji = (type) => {
    const emojis = {
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
    return emojis[type] || '📦';
  };

  return (
    <div className="dashboard">
      <Header />
      <div className="dashboard-content">
        <div className="container-dashboard">
          <div className="collection-points-container">
            <div className="list-header">
              <button className="btn-back" onClick={() => navigate('/dashboard')}>
                ← Voltar
              </button>
              <h1>Pontos de Coleta</h1>
            </div>

            <div className="points-grid">
              {collectionPoints.map((point) => (
                <div key={point.id} className="point-card">
                  <div className="point-header">
                    <h3>📍 {point.name}</h3>
                  </div>

                  <div className="point-info">
                    <div className="info-row">
                      <span className="label">Endereço:</span>
                      <span className="value">
                        {point.address}, {point.city} - {point.state}
                      </span>
                    </div>

                    <div className="info-row">
                      <span className="label">Telefone:</span>
                      <a href={`tel:${point.phone}`} className="value phone">
                        {point.phone}
                      </a>
                    </div>

                    <div className="info-row">
                      <span className="label">Horário:</span>
                      <span className="value">{point.hours}</span>
                    </div>

                    <div className="info-row">
                      <span className="label">Tipos Aceitos:</span>
                      <div className="waste-types">
                        {point.types.map((type) => (
                          <span key={type} className="waste-badge">
                            {getTypeEmoji(type)} {type}
                          </span>
                        ))}
                      </div>
                    </div>
                  </div>

                  <div className="point-actions">
                    <button className="btn btn-secondary">Ver Mapa</button>
                    <button className="btn btn-primary">Agendar Coleta</button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
