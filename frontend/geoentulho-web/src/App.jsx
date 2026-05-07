import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import Landing from './pages/Landing';
import Clients from './pages/Clients';
import Login from './pages/Login';
import Register from './pages/Register';
import Profile from './pages/Profile';
import Home from './pages/Home';
import CreateTicket from './pages/CreateTicket';
import MyTickets from './pages/MyTickets';
import OpenTickets from './pages/OpenTickets';
import CollectionPoints from './pages/CollectionPoints';
import Reports from './pages/Reports';
import './styles/global.css';

// Componente para rotas protegidas
function ProtectedRoute({ children }) {
  const { isAuthenticated } = useAuth();
  
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }
  
  return children;
}

// Componente principal com rotas
function AppContent() {
  return (
    <Routes>
      <Route path="/" element={<Landing />} />
      <Route path="/clientes" element={<Clients />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route
        path="/dashboard"
        element={
          <ProtectedRoute>
            <Home />
          </ProtectedRoute>
        }
      />
      <Route
        path="/profile"
        element={
          <ProtectedRoute>
            <Profile />
          </ProtectedRoute>
        }
      />
      <Route
        path="/create-ticket"
        element={
          <ProtectedRoute>
            <CreateTicket />
          </ProtectedRoute>
        }
      />
      <Route
        path="/my-tickets"
        element={
          <ProtectedRoute>
            <MyTickets />
          </ProtectedRoute>
        }
      />
      <Route
        path="/open-tickets"
        element={
          <ProtectedRoute>
            <OpenTickets />
          </ProtectedRoute>
        }
      />
      <Route
        path="/collection-points"
        element={
          <ProtectedRoute>
            <CollectionPoints />
          </ProtectedRoute>
        }
      />
      <Route
        path="/reports"
        element={
          <ProtectedRoute>
            <Reports />
          </ProtectedRoute>
        }
      />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

// Aplicação com provider
export default function App() {
  return (
    <Router>
      <AuthProvider>
        <AppContent />
      </AuthProvider>
    </Router>
  );
}
