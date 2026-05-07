import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import '../styles/auth.css';

export default function Register() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [name, setName] = useState('');
  const [type, setType] = useState('citizen');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await register(email, password, name, type);
      navigate('/login');
    } catch (err) {
      setError(err.response?.data?.message || 'Erro ao registrar');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-box">
        <h1>GeoEntulho</h1>
        <h2>Registrar</h2>
        
        {error && <div className="error">{error}</div>}

        <form onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Nome"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
          <input
            type="password"
            placeholder="Senha"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />

          {/* Seletor de tipo de usuário com cards */}
          <div className="user-type-selector">
            <label>Tipo de Conta *</label>
            <div className="type-options">
              <div 
                className={`type-option ${type === 'citizen' ? 'active' : ''}`}
                onClick={() => setType('citizen')}
              >
                <div className="type-icon">👤</div>
                <div className="type-label">Cidadão</div>
                <small>Solicitar coleta de resíduos</small>
              </div>
              <div 
                className={`type-option ${type === 'company' ? 'active' : ''}`}
                onClick={() => setType('company')}
              >
                <div className="type-icon">🏢</div>
                <div className="type-label">Empresa</div>
                <small>Prestar serviços de coleta</small>
              </div>
            </div>
          </div>

          <button type="submit" disabled={loading}>
            {loading ? 'Registrando...' : 'Registrar'}
          </button>
        </form>

        <p>
          Já tem conta? <Link to="/login">Faça login aqui</Link>
        </p>
      </div>
    </div>
  );
}
