import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Header from '../components/Header';
import '../styles/ticket.css';

export default function Reports() {
  const { user } = useAuth();
  const navigate = useNavigate();

  if (!user || user.type !== 'company') {
    navigate('/dashboard');
    return null;
  }

  const reports = [
    {
      id: 1,
      title: 'Coletas por Região',
      icon: '🗺️',
      description: 'Análise de coletas realizadas em diferentes regiões',
      data: [
        { region: 'Centro', count: 45, percentage: 35 },
        { region: 'Savassi', count: 38, percentage: 29 },
        { region: 'Pampulha', count: 42, percentage: 32 },
      ],
    },
    {
      id: 2,
      title: 'Coletas por Tipo de Resíduo',
      icon: '♻️',
      description: 'Distribuição dos resíduos coletados por tipo',
      data: [
        { type: 'Construção', count: 52, percentage: 40 },
        { type: 'Eletrônico', count: 38, percentage: 29 },
        { type: 'Orgânico', count: 35, percentage: 27 },
      ],
    },
    {
      id: 3,
      title: 'Status dos Chamados',
      icon: '📊',
      description: 'Quantidade de chamados por status',
      data: [
        { status: 'Concluído', count: 89, percentage: 68 },
        { status: 'Em Coleta', count: 22, percentage: 17 },
        { status: 'Aceito', count: 19, percentage: 15 },
      ],
    },
  ];

  return (
    <div className="dashboard">
      <Header />
      <div className="dashboard-content">
        <div className="container-dashboard">
          <div className="reports-container">
            <div className="list-header">
              <button className="btn-back" onClick={() => navigate('/dashboard')}>
                ← Voltar
              </button>
              <h1>Relatórios</h1>
            </div>

            <div className="reports-grid">
              {reports.map((report) => (
                <div key={report.id} className="report-card">
                  <div className="report-header">
                    <span className="report-icon">{report.icon}</span>
                    <div>
                      <h3>{report.title}</h3>
                      <p>{report.description}</p>
                    </div>
                  </div>

                  <div className="report-content">
                    <table className="report-table">
                      <tbody>
                        {report.data.map((row, idx) => {
                          const key = row.region || row.type || row.status;
                          return (
                            <tr key={idx}>
                              <td>{key}</td>
                              <td className="numeric">{row.count}</td>
                              <td>
                                <div className="progress-bar">
                                  <div
                                    className="progress"
                                    style={{
                                      width: `${row.percentage}%`,
                                      backgroundColor: `hsl(${(idx * 120) % 360}, 70%, 50%)`,
                                    }}
                                  />
                                </div>
                              </td>
                              <td className="percentage">{row.percentage}%</td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                </div>
              ))}
            </div>

            <div className="report-summary">
              <h2>Resumo Geral</h2>
              <div className="summary-cards">
                <div className="summary-card">
                  <span className="summary-icon">📦</span>
                  <div className="summary-content">
                    <span className="label">Total de Coletas</span>
                    <span className="value">130</span>
                  </div>
                </div>

                <div className="summary-card">
                  <span className="summary-icon">⚖️</span>
                  <div className="summary-content">
                    <span className="label">Peso Total Coletado</span>
                    <span className="value">45.2 ton</span>
                  </div>
                </div>

                <div className="summary-card">
                  <span className="summary-icon">✅</span>
                  <div className="summary-content">
                    <span className="label">Taxa de Conclusão</span>
                    <span className="value">68%</span>
                  </div>
                </div>

                <div className="summary-card">
                  <span className="summary-icon">💰</span>
                  <div className="summary-content">
                    <span className="label">Receita Gerada</span>
                    <span className="value">R$ 12.500</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
