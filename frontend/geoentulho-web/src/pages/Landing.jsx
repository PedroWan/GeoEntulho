import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/landing.css';

export default function Landing() {
  const navigate = useNavigate();
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  const scrollToSection = (sectionId) => {
    setMobileMenuOpen(false);
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  };

  return (
    <div className="landing">
      {/* Header */}
      <header className="header">
        <div className="header-top">
          <div className="container-header">
            <div className="contact-info">
              <a href="tel:+553135081919" className="contact-link">
                📞 31 3508.1919
              </a>
              <a href="mailto:inovar@inovarambiental.com.br" className="contact-link">
                ✉️ contato@geoentulho.com.br
              </a>
            </div>
            <div className="social-links">
              <a href="#" title="LinkedIn" className="social-icon">💼</a>
              <a href="#" title="Instagram" className="social-icon">📸</a>
              <a href="#" title="Facebook" className="social-icon">👥</a>
              <a href="#" title="YouTube" className="social-icon">▶️</a>
            </div>
          </div>
        </div>

        <nav className="navbar">
          <div className="container-header">
            <div className="logo">
              <h1>GeoEntulho</h1>
            </div>

            <button 
              className="menu-toggle"
              onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
            >
              ☰
            </button>

            <ul className={`menu ${mobileMenuOpen ? 'open' : ''}`}>
              <li>
                <button onClick={() => scrollToSection('hero')} className="menu-link">
                  INÍCIO
                </button>
              </li>
              <li>
                <button onClick={() => scrollToSection('servicos')} className="menu-link">
                  SERVIÇOS
                </button>
              </li>
              <li>
                <button onClick={() => scrollToSection('sobre')} className="menu-link">
                  SOBRE
                </button>
              </li>
              <li>
                <button onClick={() => scrollToSection('contato')} className="menu-link">
                  CONTATO
                </button>
              </li>
            </ul>

            <button 
              className="btn-primary"
              onClick={() => navigate('/login')}
            >
              ÁREA DO CLIENTE
            </button>
          </div>
        </nav>
      </header>

      {/* Hero Section */}
      <section id="hero" className="hero">
        <div className="hero-overlay"></div>
        <div className="hero-content">
          <h2>Gestão sustentável de resíduos</h2>
          <p>Soluções inteligentes para coleta, transporte e destinação de resíduos</p>
          <button 
            className="btn-cta"
            onClick={() => navigate('/clientes')}
          >
            SOLICITE UM ORÇAMENTO
          </button>
        </div>
      </section>

      {/* Serviços Section */}
      <section id="servicos" className="servicos">
        <div className="container-header">
          <h2 className="section-title">Nossos Serviços</h2>
          <div className="servicos-grid">
            <div className="service-card">
              <div className="service-icon">📦</div>
              <h3>Coleta</h3>
              <p>Coleta eficiente de resíduos em sua localização com agendamento flexível</p>
            </div>

            <div className="service-card">
              <div className="service-icon">🚚</div>
              <h3>Transporte</h3>
              <p>Transporte seguro e rastreável para todos os tipos de resíduos</p>
            </div>

            <div className="service-card">
              <div className="service-icon">♻️</div>
              <h3>Tratamento</h3>
              <p>Tratamento especializado com tecnologias modernas e sustentáveis</p>
            </div>

            <div className="service-card">
              <div className="service-icon">🏭</div>
              <h3>Destinação</h3>
              <p>Destinação adequada de acordo com normas ambientais vigentes</p>
            </div>

            <div className="service-card">
              <div className="service-icon">📍</div>
              <h3>Pontos de Coleta</h3>
              <p>Rede de pontos de coleta estratégicos para sua conveniência</p>
            </div>

            <div className="service-card">
              <div className="service-icon">📊</div>
              <h3>Relatórios</h3>
              <p>Acompanhamento detalhado e relatórios de todas as operações</p>
            </div>
          </div>
        </div>
      </section>

      {/* Sobre Section */}
      <section id="sobre" className="sobre">
        <div className="container-header">
          <h2 className="section-title">Sobre o GeoEntulho</h2>
          <div className="sobre-content">
            <div className="sobre-text">
              <p>
                O GeoEntulho é uma plataforma inovadora de gestão de resíduos que conecta
                cidadãos e empresas com soluções sustentáveis de coleta e destinação.
              </p>
              <p>
                Com tecnologia de ponta e compromisso ambiental, transformamos a forma como
                os resíduos são gerenciados, garantindo práticas responsáveis e sustentáveis.
              </p>
              <p>
                Nossa missão é facilitar a vida das pessoas enquanto protegemos o meio ambiente.
              </p>
            </div>
            <div className="sobre-stats">
              <div className="stat">
                <h4>500+</h4>
                <p>Usuários Ativos</p>
              </div>
              <div className="stat">
                <h4>100+</h4>
                <p>Pontos de Coleta</p>
              </div>
              <div className="stat">
                <h4>50</h4>
                <p>Empresas Parceiras</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Contato Section */}
      <section id="contato" className="contato">
        <div className="container-header">
          <h2 className="section-title">Quer ser um parceiro? Entre em Contato</h2>
          <div className="contato-content">
            <div className="contato-info">
              <h3>📞 Telefone</h3>
              <a href="tel:+553135081919">(31) 3508-1919</a>
              
              <h3 style={{ marginTop: '2rem' }}>📧 Email</h3>
              <a href="mailto:contato@geoentulho.com.br">contato@geoentulho.com.br</a>
              
              <h3 style={{ marginTop: '2rem' }}>📍 Localização</h3>
              <p>Belo Horizonte, MG - Brasil</p>
            </div>

            <form className="contato-form" onSubmit={(e) => e.preventDefault()}>
              <div className="form-group">
                <label>Nome *</label>
                <input type="text" placeholder="Seu nome" required />
              </div>

              <div className="form-group">
                <label>Email *</label>
                <input type="email" placeholder="seu@email.com" required />
              </div>

              <div className="form-group">
                <label>Mensagem *</label>
                <textarea placeholder="Sua mensagem..." rows="5" required></textarea>
              </div>

              <button type="submit" className="btn-primary">
                ENVIAR MENSAGEM
              </button>
            </form>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="footer">
        <div className="container-header">
          <div className="footer-content">
            <div className="footer-section">
              <h4>GeoEntulho</h4>
              <p>Gestão inteligente de resíduos para um futuro sustentável</p>
            </div>

            <div className="footer-section">
              <h4>Links Rápidos</h4>
              <ul>
                <li><button onClick={() => scrollToSection('servicos')} className="footer-link">Serviços</button></li>
                <li><button onClick={() => scrollToSection('sobre')} className="footer-link">Sobre</button></li>
                <li><button onClick={() => scrollToSection('contato')} className="footer-link">Contato</button></li>
                <li><button onClick={() => navigate('/login')} className="footer-link">Login</button></li>
              </ul>
            </div>

            <div className="footer-section">
              <h4>Redes Sociais</h4>
              <div className="social-links-footer">
                <a href="#" title="LinkedIn" className="social-icon-footer">💼 LinkedIn</a>
                <a href="#" title="Instagram" className="social-icon-footer">📸 Instagram</a>
                <a href="#" title="Facebook" className="social-icon-footer">👥 Facebook</a>
              </div>
            </div>
          </div>

          <div className="footer-bottom">
            <p>&copy; 2026 GeoEntulho. Todos os direitos reservados.</p>
          </div>
        </div>
      </footer>
    </div>
  );
}
