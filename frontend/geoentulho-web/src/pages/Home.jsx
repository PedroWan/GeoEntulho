import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import '../styles/home.css';

export default function Home() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  if (!user) {
    navigate('/login');
    return null;
  }

  return (
    <div className="dashboard-container">
      <div className="dashboard-header">
        <h1>🌍 GeoEntulho</h1>
        <button onClick={handleLogout}>Sair</button>
      </div>

      <div className="dashboard-content">
        <div className="welcome-card">
          <h2>Bem-vindo(a), {user.name}!</h2>
          <div className="user-info">
            <p>
              <strong>Email:</strong> {user.email}
            </p>
            <p>
              <strong>Tipo de acesso:</strong> {user.type === 'citizen' ? 'Cidadão' : 'Empresa de Coleta'}
            </p>
          </div>
        </div>

        {user.type === 'citizen' ? (
          <div className="features-grid">
            <div className="feature-card citizen-features">
              <h3>🗺️ Criar Chamado</h3>
              <p>Solicite coleta de resíduos ou descarte em pontos de coleta designados.</p>
            </div>
            <div className="feature-card citizen-features">
              <h3>📋 Meus Chamados</h3>
              <p>Visualize o status de suas solicitações em tempo real.</p>
            </div>
            <div className="feature-card citizen-features">
              <h3>📍 Pontos de Coleta</h3>
              <p>Encontre pontos de coleta próximos a você no mapa.</p>
            </div>
          </div>
        ) : (
          <div className="features-grid">
            <div className="feature-card company-features">
              <h3>📊 Dashboard</h3>
              <p>Visualize chamados pendentes e histórico de coletas.</p>
            </div>
            <div className="feature-card company-features">
              <h3>✅ Gerenciar</h3>
              <p>Aceite, atualize e conclua chamados de coleta.</p>
            </div>
            <div className="feature-card company-features">
              <h3>📈 Relatórios</h3>
              <p>Análise de dados de descarte por região e tipo.</p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
