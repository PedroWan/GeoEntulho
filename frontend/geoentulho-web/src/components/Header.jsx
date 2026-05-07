import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import '../styles/header.css';

export default function Header() {
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
  const [userMenuOpen, setUserMenuOpen] = useState(false);
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <header className="dashboard-header">
      <div className="header-container">
        <div className="logo-section" onClick={() => navigate('/')}>
          <h1 className="logo">🌍 GeoEntulho</h1>
        </div>

        <nav className="header-nav">
          <button className="nav-item active">
            Dashboard
          </button>
          {user?.type === 'citizen' ? (
            <>
              <button className="nav-item">
                Meus Chamados
              </button>
              <button className="nav-item">
                Pontos de Coleta
              </button>
            </>
          ) : (
            <>
              <button className="nav-item">
                Coletas
              </button>
              <button className="nav-item">
                Relatórios
              </button>
            </>
          )}
        </nav>

        <div className="user-section">
          <div className="user-menu-trigger" onClick={() => setUserMenuOpen(!userMenuOpen)}>
            <div className="user-avatar">
              {user?.name?.charAt(0).toUpperCase()}
            </div>
            <div className="user-info">
              <span className="user-name">{user?.name}</span>
              <span className="user-type">
                {user?.type === 'citizen' ? '👤 Cidadão' : '🏢 Empresa'}
              </span>
            </div>
            <span className="dropdown-arrow">▼</span>
          </div>

          {userMenuOpen && (
            <div className="user-dropdown">
              <div className="dropdown-header">
                <strong>{user?.name}</strong>
                <p>{user?.email}</p>
              </div>
              <div className="dropdown-divider"></div>
              <button 
                className="dropdown-item"
                onClick={() => {
                  navigate('/profile');
                  setUserMenuOpen(false);
                }}
              >
                👤 Meu Perfil
              </button>
              <button className="dropdown-item">
                ⚙️ Configurações
              </button>
              <div className="dropdown-divider"></div>
              <button 
                className="dropdown-item logout"
                onClick={handleLogout}
              >
                🚪 Sair
              </button>
            </div>
          )}
        </div>

        <button 
          className="menu-toggle"
          onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
        >
          ☰
        </button>
      </div>

      {mobileMenuOpen && (
        <nav className="mobile-menu">
          <button className="mobile-nav-item">Dashboard</button>
          {user?.type === 'citizen' ? (
            <>
              <button className="mobile-nav-item">Meus Chamados</button>
              <button className="mobile-nav-item">Pontos de Coleta</button>
            </>
          ) : (
            <>
              <button className="mobile-nav-item">Coletas</button>
              <button className="mobile-nav-item">Relatórios</button>
            </>
          )}
          <div className="mobile-divider"></div>
          <button 
            className="mobile-nav-item"
            onClick={() => {
              navigate('/profile');
              setMobileMenuOpen(false);
            }}
          >
            👤 Meu Perfil
          </button>
          <button className="mobile-nav-item">⚙️ Configurações</button>
          <button 
            className="mobile-nav-item logout"
            onClick={handleLogout}
          >
            🚪 Sair
          </button>
        </nav>
      )}
    </header>
  );
}
