import { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import api from '../services/api';
import '../styles/profile.css';

export default function Profile() {
  const { user, logout } = useAuth();
  const [profile, setProfile] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [formData, setFormData] = useState({
    name: '',
    phone: '',
    bio: '',
    address: '',
    city: '',
    state: '',
    zipCode: '',
    companyName: '',
    companyWebsite: ''
  });

  useEffect(() => {
    loadProfile();
  }, []);

  const loadProfile = async () => {
    try {
      setLoading(true);
      const response = await api.get('/auth/profile');
      setProfile(response.data);
      setFormData({
        name: response.data.name || '',
        phone: response.data.phone || '',
        bio: response.data.bio || '',
        address: response.data.address || '',
        city: response.data.city || '',
        state: response.data.state || '',
        zipCode: response.data.zipCode || '',
        companyName: response.data.companyName || '',
        companyWebsite: response.data.companyWebsite || ''
      });
      setError('');
    } catch (err) {
      setError('Erro ao carregar perfil');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSave = async () => {
    try {
      setLoading(true);
      await api.put('/auth/profile', formData);
      setIsEditing(false);
      await loadProfile();
      setError('');
    } catch (err) {
      setError('Erro ao atualizar perfil');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    setIsEditing(false);
    loadProfile();
  };

  if (loading && !profile) {
    return <div className="profile-container"><p>Carregando...</p></div>;
  }

  return (
    <div className="profile-container">
      <div className="profile-header">
        <div className="profile-avatar">
          {profile?.photoUrl ? (
            <img src={profile.photoUrl} alt={profile.name} />
          ) : (
            <div className="avatar-placeholder">{profile?.name?.charAt(0).toUpperCase()}</div>
          )}
        </div>
        <div className="header-info">
          <h1>{profile?.name}</h1>
          <p className="profile-email">{profile?.email}</p>
          <span className="profile-badge">
            {profile?.type === 'citizen' ? '👤 Cidadão' : '🏢 Empresa'}
          </span>
          {profile?.isVerified && <span className="verified-badge">✓ Verificado</span>}
        </div>
        {!isEditing && (
          <button className="btn-edit" onClick={() => setIsEditing(true)}>
            ✏️ Editar Perfil
          </button>
        )}
      </div>

      {error && <div className="error-message">{error}</div>}

      {isEditing ? (
        <div className="profile-form">
          <div className="form-section">
            <h3>Informações Pessoais</h3>
            <div className="form-group">
              <label>Nome *</label>
              <input 
                type="text" 
                name="name"
                value={formData.name} 
                onChange={handleChange}
                placeholder="Seu nome completo"
              />
            </div>
            <div className="form-group">
              <label>Telefone</label>
              <input 
                type="tel" 
                name="phone"
                value={formData.phone} 
                onChange={handleChange}
                placeholder="(31) 9999-9999"
              />
            </div>
            <div className="form-group">
              <label>Bio</label>
              <textarea 
                name="bio"
                value={formData.bio} 
                onChange={handleChange}
                placeholder="Conte-nos um pouco sobre você..."
                rows="4"
              />
            </div>
          </div>

          <div className="form-section">
            <h3>Endereço</h3>
            <div className="form-group">
              <label>Endereço</label>
              <input 
                type="text" 
                name="address"
                value={formData.address} 
                onChange={handleChange}
                placeholder="Rua, número..."
              />
            </div>
            <div className="form-row">
              <div className="form-group">
                <label>Cidade</label>
                <input 
                  type="text" 
                  name="city"
                  value={formData.city} 
                  onChange={handleChange}
                  placeholder="Cidade"
                />
              </div>
              <div className="form-group">
                <label>Estado</label>
                <input 
                  type="text" 
                  name="state"
                  value={formData.state} 
                  onChange={handleChange}
                  placeholder="MG"
                />
              </div>
              <div className="form-group">
                <label>CEP</label>
                <input 
                  type="text" 
                  name="zipCode"
                  value={formData.zipCode} 
                  onChange={handleChange}
                  placeholder="30000-000"
                />
              </div>
            </div>
          </div>

          {profile?.type === 'company' && (
            <div className="form-section">
              <h3>Informações da Empresa</h3>
              <div className="form-group">
                <label>Nome da Empresa</label>
                <input 
                  type="text" 
                  name="companyName"
                  value={formData.companyName} 
                  onChange={handleChange}
                  placeholder="Nome da sua empresa"
                />
              </div>
              <div className="form-group">
                <label>Website</label>
                <input 
                  type="url" 
                  name="companyWebsite"
                  value={formData.companyWebsite} 
                  onChange={handleChange}
                  placeholder="https://exemplo.com"
                />
              </div>
            </div>
          )}

          <div className="form-actions">
            <button className="btn-save" onClick={handleSave} disabled={loading}>
              {loading ? 'Salvando...' : 'Salvar Mudanças'}
            </button>
            <button className="btn-cancel" onClick={handleCancel} disabled={loading}>
              Cancelar
            </button>
          </div>
        </div>
      ) : (
        <div className="profile-info">
          <div className="info-section">
            <h3>Informações Pessoais</h3>
            <div className="info-row">
              <span className="label">Email:</span>
              <span className="value">{profile?.email}</span>
            </div>
            {profile?.phone && (
              <div className="info-row">
                <span className="label">Telefone:</span>
                <span className="value">{profile.phone}</span>
              </div>
            )}
            {profile?.bio && (
              <div className="info-row">
                <span className="label">Bio:</span>
                <span className="value">{profile.bio}</span>
              </div>
            )}
          </div>

          {(profile?.address || profile?.city) && (
            <div className="info-section">
              <h3>Endereço</h3>
              {profile?.address && (
                <div className="info-row">
                  <span className="label">Endereço:</span>
                  <span className="value">{profile.address}</span>
                </div>
              )}
              {profile?.city && (
                <div className="info-row">
                  <span className="label">Cidade:</span>
                  <span className="value">{profile.city}, {profile.state}</span>
                </div>
              )}
              {profile?.zipCode && (
                <div className="info-row">
                  <span className="label">CEP:</span>
                  <span className="value">{profile.zipCode}</span>
                </div>
              )}
            </div>
          )}

          {profile?.type === 'company' && (profile?.companyName || profile?.companyWebsite) && (
            <div className="info-section">
              <h3>Empresa</h3>
              {profile?.companyName && (
                <div className="info-row">
                  <span className="label">Nome:</span>
                  <span className="value">{profile.companyName}</span>
                </div>
              )}
              {profile?.companyWebsite && (
                <div className="info-row">
                  <span className="label">Website:</span>
                  <a href={profile.companyWebsite} target="_blank" rel="noopener noreferrer" className="value link">
                    {profile.companyWebsite}
                  </a>
                </div>
              )}
            </div>
          )}

          <div className="profile-meta">
            <small>Membro desde: {new Date(profile?.createdAt).toLocaleDateString('pt-BR')}</small>
          </div>
        </div>
      )}
    </div>
  );
}
