import { useNavigate } from 'react-router-dom';
import '../styles/clients.css';

export default function Clients() {
  const navigate = useNavigate();

  return (
    <div className="clients-page">
      {/* Header Simples */}
      <header className="clients-header">
        <div className="clients-header-content">
          <button 
            className="clients-logo"
            onClick={() => navigate('/')}
          >
            🌍 GeoEntulho
          </button>
          <button 
            className="btn-back"
            onClick={() => navigate('/')}
          >
            ← Voltar
          </button>
        </div>
      </header>

      {/* Hero para Clientes */}
      <section className="clients-hero">
        <div className="clients-container">
          <h1>Bem-vindo(a) ao GeoEntulho!</h1>
          <p>Transforme a forma como você gerencia resíduos</p>
        </div>
      </section>

      {/* Benefícios */}
      <section className="clients-benefits">
        <div className="clients-container">
          <h2>Por que escolher GeoEntulho?</h2>
          
          <div className="benefits-grid">
            <div className="benefit-card">
              <div className="benefit-icon">🚀</div>
              <h3>Rápido e Fácil</h3>
              <p>Solicite coleta em poucos cliques e acompanhe em tempo real</p>
            </div>

            <div className="benefit-card">
              <div className="benefit-icon">🌱</div>
              <h3>Sustentável</h3>
              <p>Destinação correta com responsabilidade ambiental</p>
            </div>

            <div className="benefit-card">
              <div className="benefit-icon">💰</div>
              <h3>Preços Competitivos</h3>
              <p>Orçamentos personalizados e sem taxas ocultas</p>
            </div>

            <div className="benefit-card">
              <div className="benefit-icon">🛡️</div>
              <h3>Segurança</h3>
              <p>Dados protegidos e conformidade com legislação ambiental</p>
            </div>

            <div className="benefit-card">
              <div className="benefit-icon">📊</div>
              <h3>Relatórios</h3>
              <p>Acompanhe todas as suas movimentações de resíduos</p>
            </div>

            <div className="benefit-card">
              <div className="benefit-icon">🤝</div>
              <h3>Suporte</h3>
              <p>Equipe dedicada para atender suas necessidades</p>
            </div>
          </div>
        </div>
      </section>

      {/* Como Funciona */}
      <section className="clients-how-it-works">
        <div className="clients-container">
          <h2>Como Funciona?</h2>
          
          <div className="steps-container">
            <div className="step">
              <div className="step-number">1</div>
              <h3>Registre-se</h3>
              <p>Crie sua conta informando seus dados</p>
            </div>

            <div className="step-arrow">→</div>

            <div className="step">
              <div className="step-number">2</div>
              <h3>Solicite Coleta</h3>
              <p>Descreva seus resíduos e locação</p>
            </div>

            <div className="step-arrow">→</div>

            <div className="step">
              <div className="step-number">3</div>
              <h3>Acompanhe</h3>
              <p>Receba atualizações em tempo real</p>
            </div>

            <div className="step-arrow">→</div>

            <div className="step">
              <div className="step-number">4</div>
              <h3>Feito!</h3>
              <p>Resíduos coletados e destinados</p>
            </div>
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="clients-cta">
        <div className="clients-container">
          <h2>Pronto para começar?</h2>
          <p>Junte-se a centenas de clientes satisfeitos</p>
          
          <div className="cta-buttons">
            <button 
              className="btn-register"
              onClick={() => navigate('/register')}
            >
              Criar Conta Grátis
            </button>
            <button 
              className="btn-login"
              onClick={() => navigate('/login')}
            >
              Já tenho conta
            </button>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="clients-footer">
        <div className="clients-container">
          <p>&copy; 2026 GeoEntulho. Todos os direitos reservados.</p>
        </div>
      </footer>
    </div>
  );
}
