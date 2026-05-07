import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Header from '../components/Header';
import api from '../services/api';
import '../styles/ticket.css';

export default function CreateTicket() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    wasteType: 'construção',
    address: '',
    city: '',
    state: '',
    phone: '',
    estimatedWeight: '',
  });

  if (!user || user.type !== 'citizen') {
    navigate('/dashboard');
    return null;
  }

  const wasteTypes = [
    { value: 'construção', label: '🏗️ Construção' },
    { value: 'eletrônico', label: '📱 Eletrônico' },
    { value: 'orgânico', label: '🌱 Orgânico' },
    { value: 'plástico', label: '♻️ Plástico' },
    { value: 'metal', label: '⚙️ Metal' },
    { value: 'vidro', label: '🔍 Vidro' },
    { value: 'papel', label: '📄 Papel' },
    { value: 'madeira', label: '🪵 Madeira' },
    { value: 'outros', label: '📦 Outros' },
  ];

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      // Validar campos obrigatórios
      if (!formData.title || !formData.address || !formData.city || !formData.state) {
        setError('Preencha todos os campos obrigatórios');
        setLoading(false);
        return;
      }

      const ticketData = {
        title: formData.title,
        description: formData.description,
        wasteType: formData.wasteType,
        address: formData.address,
        city: formData.city,
        state: formData.state,
        phone: formData.phone,
        estimatedWeight: formData.estimatedWeight ? parseFloat(formData.estimatedWeight) : null,
      };

      const response = await api.post('/api/tickets', ticketData);

      if (response.data.success || response.status === 200) {
        setSuccess('Chamado criado com sucesso! Redirecionando...');
        setTimeout(() => {
          navigate('/my-tickets');
        }, 2000);
      }
    } catch (err) {
      const message = err.response?.data?.message || err.message || 'Erro ao criar chamado';
      setError(message);
      console.error('Erro ao criar chamado:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="dashboard">
      <Header />
      <div className="dashboard-content">
        <div className="container-dashboard">
          <div className="ticket-form-container">
            <div className="form-header">
              <button className="btn-back" onClick={() => navigate('/dashboard')}>
                ← Voltar
              </button>
              <h1>Criar Novo Chamado</h1>
            </div>

            {error && <div className="alert alert-error">{error}</div>}
            {success && <div className="alert alert-success">{success}</div>}

            <form onSubmit={handleSubmit} className="ticket-form">
              <div className="form-section">
                <h2>Informações do Chamado</h2>

                <div className="form-group">
                  <label htmlFor="title">Título *</label>
                  <input
                    type="text"
                    id="title"
                    name="title"
                    placeholder="Ex: Coleta de entulho da reforma"
                    value={formData.title}
                    onChange={handleChange}
                    required
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="description">Descrição</label>
                  <textarea
                    id="description"
                    name="description"
                    placeholder="Descreva detalhes sobre o resíduo..."
                    rows="4"
                    value={formData.description}
                    onChange={handleChange}
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="wasteType">Tipo de Resíduo *</label>
                  <select
                    id="wasteType"
                    name="wasteType"
                    value={formData.wasteType}
                    onChange={handleChange}
                    required
                  >
                    {wasteTypes.map((type) => (
                      <option key={type.value} value={type.value}>
                        {type.label}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="form-group">
                  <label htmlFor="estimatedWeight">Peso Estimado (kg)</label>
                  <input
                    type="number"
                    id="estimatedWeight"
                    name="estimatedWeight"
                    placeholder="Ex: 500"
                    step="0.1"
                    min="0"
                    value={formData.estimatedWeight}
                    onChange={handleChange}
                  />
                </div>
              </div>

              <div className="form-section">
                <h2>Localização</h2>

                <div className="form-group">
                  <label htmlFor="address">Endereço *</label>
                  <input
                    type="text"
                    id="address"
                    name="address"
                    placeholder="Rua, número e complemento"
                    value={formData.address}
                    onChange={handleChange}
                    required
                  />
                </div>

                <div className="form-row">
                  <div className="form-group">
                    <label htmlFor="city">Cidade *</label>
                    <input
                      type="text"
                      id="city"
                      name="city"
                      placeholder="Ex: Belo Horizonte"
                      value={formData.city}
                      onChange={handleChange}
                      required
                    />
                  </div>

                  <div className="form-group">
                    <label htmlFor="state">Estado/UF *</label>
                    <input
                      type="text"
                      id="state"
                      name="state"
                      placeholder="Ex: MG"
                      maxLength="2"
                      value={formData.state}
                      onChange={handleChange}
                      required
                    />
                  </div>
                </div>

                <div className="form-group">
                  <label htmlFor="phone">Telefone para Contato</label>
                  <input
                    type="tel"
                    id="phone"
                    name="phone"
                    placeholder="(31) 99999-9999"
                    value={formData.phone}
                    onChange={handleChange}
                  />
                </div>
              </div>

              <div className="form-actions">
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={() => navigate('/dashboard')}
                  disabled={loading}
                >
                  Cancelar
                </button>
                <button
                  type="submit"
                  className="btn btn-primary"
                  disabled={loading}
                >
                  {loading ? 'Criando...' : 'Criar Chamado'}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
}
