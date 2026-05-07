import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import '../styles/home.css';

export default function Home() {
  const { user } = useAuth();
  const navigate = useNavigate();

  if (!user) {
    navigate('/login');
    return null;
  }

  return (
    <div className="dashboard">
      <Header />
      <div className="dashboard-content">
        <div className="container-dashboard">
          <div className="welcome-card">
            <h2>Bem-vindo(a), {user.name}! 👋</h2>
            <p>Gerencie suas atividades de forma eficiente</p>
          </div>

          {user.type === 'citizen' ? (
            <div className="features-grid">
              <div className="feature-card citizen-features">
                <div className="feature-icon">🗺️</div>
                <h3>Criar Chamado</h3>
                <p>Solicite coleta de resíduos ou descarte em pontos de coleta designados.</p>
                <button className="btn-feature" onClick={() => navigate('/create-ticket')}>Criar</button>
              </div>
              <div className="feature-card citizen-features">
                <div className="feature-icon">📋</div>
                <h3>Meus Chamados</h3>
                <p>Visualize o status de suas solicitações em tempo real.</p>
                <button className="btn-feature" onClick={() => navigate('/my-tickets')}>Visualizar</button>
              </div>
              <div className="feature-card citizen-features">
                <div className="feature-icon">📍</div>
                <h3>Pontos de Coleta</h3>
                <p>Encontre pontos de coleta próximos a você no mapa.</p>
                <button className="btn-feature" onClick={() => navigate('/collection-points')}>Explorar</button>
              </div>
            </div>
          ) : (
            <div className="features-grid">
              <div className="feature-card company-features">
                <div className="feature-icon">📊</div>
                <h3>Dashboard</h3>
                <p>Visualize chamados pendentes e histórico de coletas.</p>
                <button className="btn-feature" onClick={() => navigate('/dashboard')}>Ver</button>
              </div>
              <div className="feature-card company-features">
                <div className="feature-icon">✅</div>
                <h3>Gerenciar Chamados</h3>
                <p>Aceite, atualize e conclua chamados de coleta.</p>
                <button className="btn-feature" onClick={() => navigate('/open-tickets')}>Gerenciar</button>
              </div>
              <div className="feature-card company-features">
                <div className="feature-icon">📈</div>
                <h3>Relatórios</h3>
                <p>Análise de dados de descarte por região e tipo.</p>
                <button className="btn-feature" onClick={() => navigate('/reports')}>Acessar</button>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
