# 🌍 GeoEntulho - Smart Cities Waste Management Platform

**A sustainable solution for construction waste management in smart cities**

---

## 📖 Overview

GeoEntulho is an MVP platform that connects citizens and construction waste management companies. The system enables two waste disposal workflows:

1. **Drop-off**: Citizens bring waste to designated collection points
2. **Pickup**: Companies collect waste directly from citizen locations

The platform is aligned with:
- **ODS 11**: Sustainable Cities and Communities
- **ODS 12**: Responsible Consumption and Production
- **ODS 9**: Industry, Innovation, and Infrastructure

---

## 🎯 MVP Scope (30 Days)

### Phase 1: Backend Foundation ✅
- ✅ .NET 8 Web API
- ✅ MySQL database with 5 entity models
- ✅ JWT authentication
- ✅ Database migrations

### Phase 2: Frontend Integration ✅
- ✅ React/Vite single-page application
- ✅ React Router navigation
- ✅ Authentication pages (login/register)
- ✅ Protected routes
- ✅ Dashboard (role-aware: citizen vs company)

### Phase 3: Authentication Endpoints ⏳
- ⏳ Register endpoint with BCrypt hashing
- ⏳ Login endpoint returning JWT token
- ⏳ Current user endpoint

### Phase 4: Core Features ⏳
- ⏳ Citizens can create waste tickets
- ⏳ Companies can view/accept tickets
- ⏳ Status tracking and updates
- ⏳ Leaflet maps integration

### Phase 5: Deployment ⏳
- ⏳ Railway backend deployment
- ⏳ Vercel frontend deployment
- ⏳ Production database setup
- ⏳ Public URL live

---

## 🛠️ Tech Stack

### Backend
```
.NET 8 SDK
├── ASP.NET Core Web API (RESTful)
├── Entity Framework Core 8.0 (ORM)
├── Pomelo MySQL Provider 8.0.0
├── JWT Bearer Authentication
└── CORS & OpenAPI (Swagger)
```

### Frontend
```
React 19.2.4 with Vite
├── React Router v6 (Navigation)
├── Axios (HTTP Client with JWT)
├── React Context (State Management)
├── Leaflet.js (Maps)
├── Zustand (Future store)
└── CSS3 (Responsive Design)
```

### Database
```
MySQL 9.6
├── Users (Citizens & Companies)
├── Companies (Extended user info)
├── CollectionPoints (Fixed waste drop-off locations)
├── Tickets (Waste disposal requests)
└── TicketUpdates (Status change audit trail)
```

---

## 📂 Project Structure

```
D:\GeoEntulho/
├── backend/
│   └── GeoEntulho.API/
│       ├── Models/
│       │   ├── User.cs (Auth + Identity)
│       │   ├── Company.cs (Service provider)
│       │   ├── CollectionPoint.cs (Drop-off location)
│       │   ├── Ticket.cs (Waste request)
│       │   └── TicketUpdate.cs (Status log)
│       ├── Data/
│       │   ├── ApplicationDbContext.cs
│       │   └── Migrations/
│       ├── DTOs/ (In Progress)
│       ├── Services/ (In Progress)
│       ├── Controllers/ (In Progress)
│       ├── Program.cs (Dependency Injection)
│       ├── appsettings.json (Configuration)
│       └── GeoEntulho.API.csproj
│
├── frontend/
│   └── geoentulho-web/
│       ├── src/
│       │   ├── pages/
│       │   │   ├── Login.jsx (POST /api/auth/login)
│       │   │   ├── Register.jsx (POST /api/auth/register)
│       │   │   └── Home.jsx (Protected dashboard)
│       │   ├── services/
│       │   │   ├── api.js (Axios with JWT)
│       │   │   └── authService.js (Auth API calls)
│       │   ├── context/
│       │   │   └── AuthContext.jsx (useAuth hook)
│       │   ├── styles/
│       │   │   ├── global.css
│       │   │   ├── auth.css
│       │   │   └── home.css
│       │   ├── App.jsx (Router + ProtectedRoute)
│       │   └── main.jsx (Entry point)
│       ├── public/
│       ├── .env (Local config)
│       ├── .env.example (Template)
│       ├── vite.config.js
│       └── package.json
│
├── PROGRESS.md (Development status)
└── README.md (This file)
```

---

## 🚀 Quick Start

### Prerequisites
- **Node.js 18+** with npm
- **.NET 8 SDK**
- **MySQL 9.6** server running
- **Git** (optional)

### 1. Start Backend
```bash
cd D:\GeoEntulho\backend\GeoEntulho.API

# Restore packages and run API
dotnet run

# Swagger UI opens automatically at http://localhost:5242/swagger
```

**Expected output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5242
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to stop.
```

### 2. Start Frontend (in new terminal)
```bash
cd D:\GeoEntulho\frontend\geoentulho-web

# Install dependencies (first time only)
npm install

# Start development server with hot reload
npm run dev

# Frontend available at http://localhost:5173
```

**Expected output:**
```
  vite v8.0.8 dev server running at:

  ➜  Local:   http://localhost:5173/
  ➜  press h + enter to show help
```

### 3. Test the Application
1. Open http://localhost:5173 in your browser
2. You'll be redirected to http://localhost:5173/login
3. Click "Registre-se aqui" → Go to registration page
4. Fill form:
   - **Email**: test@example.com
   - **Password**: securepass123
   - **Name**: Test User
   - **Type**: Cidadão
5. Click "Registrar" → Redirected to login
6. Fill login form with same email/password
7. Click "Entrar" → Dashboard loads showing your user info

---

## 🔐 Authentication Flow

### Registration
```
User Form Input
    ↓
Register.jsx (React)
    ↓
authService.register(email, password, name, type)
    ↓
Axios POST http://localhost:5242/api/auth/register
    ↓
Backend AuthController validates & creates User
    ↓
Returns: { success: true, message: "Registered successfully" }
    ↓
Frontend redirects to /login
```

### Login
```
User Form Input
    ↓
Login.jsx (React)
    ↓
authService.login(email, password)
    ↓
Axios POST http://localhost:5242/api/auth/login
    ↓
Backend AuthController verifies password & generates JWT
    ↓
Returns: { success: true, token: "jwt...", user: {...} }
    ↓
Frontend stores token in localStorage
    ↓
AuthContext updates (user, isAuthenticated=true)
    ↓
Redirects to / (Home.jsx)
```

### Protected Routes
```
User accesses http://localhost:5173/
    ↓
App.jsx checks ProtectedRoute
    ↓
ProtectedRoute calls useAuth() → isAuthenticated()?
    ↓
if YES: render Home.jsx
if NO:  redirect to /login
```

---

## 🔌 API Endpoints (Built Next)

### Authentication
```
POST   /api/auth/register     → Register new user
POST   /api/auth/login        → Login & get JWT token
GET    /api/auth/me           → Current user info (protected)
```

### Tickets (Coming)
```
POST   /api/tickets            → Create new ticket
GET    /api/tickets            → List my tickets
GET    /api/tickets/{id}       → Get ticket details
PUT    /api/tickets/{id}/status → Update status
```

### Companies (Coming)
```
GET    /api/companies/{id}/tickets        → Company's pending tickets
GET    /api/companies/{id}/points         → Company's collection points
```

---

## 💾 Database Schema

### Users Table
```sql
CREATE TABLE Users (
  Id             INT PRIMARY KEY AUTO_INCREMENT,
  Email          VARCHAR(255) UNIQUE NOT NULL,
  PasswordHash   LONGBLOB NOT NULL,
  Name           VARCHAR(255) NOT NULL,
  Type           VARCHAR(50) NOT NULL,  -- 'citizen' or 'company'
  Phone          VARCHAR(20),
  CreatedAt      DATETIME NOT NULL,
  UpdatedAt      DATETIME NOT NULL
);
```

### Companies Table
```sql
CREATE TABLE Companies (
  Id             INT PRIMARY KEY AUTO_INCREMENT,
  UserId         INT UNIQUE NOT NULL (FK),
  CNPJ           VARCHAR(20),
  ServiceAreaRadius INT,  -- kilometers
  Address        LONGTEXT,
  CreatedAt      DATETIME NOT NULL,
  UpdatedAt      DATETIME NOT NULL
);
```

### CollectionPoints Table
```sql
CREATE TABLE CollectionPoints (
  Id             INT PRIMARY KEY AUTO_INCREMENT,
  CompanyId      INT NOT NULL (FK),
  Name           VARCHAR(255) NOT NULL,
  Address        LONGTEXT NOT NULL,
  Latitude       DOUBLE NOT NULL,
  Longitude      DOUBLE NOT NULL,
  MaxCapacityM3  DOUBLE,
  CurrentVolumeM3 DOUBLE,
  IsActive       BIT DEFAULT 1
);
```

### Tickets Table
```sql
CREATE TABLE Tickets (
  Id               INT PRIMARY KEY AUTO_INCREMENT,
  Type             VARCHAR(50) NOT NULL,  -- 'drop_off' or 'pickup'
  Status           VARCHAR(50) NOT NULL,   -- pending, accepted, in_progress, completed, cancelled
  CitizenId        INT NOT NULL (FK),
  CompanyId        INT (FK, nullable for pending),
  CollectionPointId INT (FK, nullable),
  Address          LONGTEXT,
  Latitude         DOUBLE,
  Longitude        DOUBLE,
  ResidueType      VARCHAR(100),
  VolumeM3         DOUBLE,
  ScheduledDate    DATETIME,
  CreatedAt        DATETIME NOT NULL,
  UpdatedAt        DATETIME NOT NULL,
  CompletedAt      DATETIME
);
```

### TicketUpdates Table
```sql  
CREATE TABLE TicketUpdates (
  Id              INT PRIMARY KEY AUTO_INCREMENT,
  TicketId        INT NOT NULL (FK),
  OldStatus       VARCHAR(50),
  NewStatus       VARCHAR(50) NOT NULL,
  Message         LONGTEXT,
  UpdatedByUserId INT NOT NULL (FK),
  CreatedAt       DATETIME NOT NULL
);
```

---

## 🎨 Frontend Components

### Login.jsx
- **Route**: `/login`
- **Form**: Email + Password
- **Action**: Calls `authService.login()` → Stores JWT → Redirects to `/`
- **Validation**: Required fields, email format
- **Error handling**: Displays API error messages

### Register.jsx
- **Route**: `/register`
- **Form**: Email + Password + Name + Type selector
- **Types**: "Cidadão" (citizen) or "Empresa de Coleta" (company)
- **Action**: Calls `authService.register()` → Redirects to `/login`
- **Validation**: Email format, password 8+ chars, all fields required

### Home.jsx
- **Route**: `/` (Protected - requires authentication)
- **Displays**:
  - User email
  - User type badge
  - Welcome message personalized
  - Feature cards (role-specific)
  - Logout button
- **Citizen features** (if Type="citizen"):
  - Create waste ticket
  - View my tickets
  - Find collection points
- **Company features** (if Type="company"):
  - View pending tickets
  - Manage collections
  - View analytics

---

## 🛡️ Security Considerations

### Passwords
- ✅ Backend: Will use BCrypt hashing (implementation pending)
- ✅ Frontend: Never sends plaintext, uses HTTPS in production
- ✅ Minimum 8 characters requirement

### JWT Tokens
- ✅ Token stored in localStorage (browser)
- ✅ Sent in Authorization header: `Bearer <token>`
- ✅ 24-hour expiration
- ✅ HS256 algorithm (HMAC with SHA-256)
- ✅ Auto-refresh on 401 (redirects to login)

### CORS
- ✅ Restricted to authorized domains
- ✅ No credentials necessary (token-based)
- ✅ Only necessary HTTP methods allowed

### Environment Variables
- ✅ `.env` file for local development (ignored by git)
- ✅ `.env.example` template for team reference
- ✅ Production variables set in Railway/Vercel dashboards

---

## 🐛 Debugging

### Frontend Debugging
```bash
# 1. Open DevTools (F12)
# 2. Console tab - check for JavaScript errors
# 3. Network tab - inspect API calls to localhost:5242
# 4. Application tab - check localStorage for JWT token

# Debug token:
# localStorage.getItem('token')
# Token can be decoded at jwt.io
```

### Backend Debugging
```bash
# Enable detailed logging:
# - Program.cs already configured for verbose output
# - Watch the dotnet run terminal for database queries
# - Check MySQL logs: C:\Program Files\MySQL\MySQL Server 9.6\data\*.err
```

### Database Debugging
```bash
# Connect via MySQL Workbench or CLI:
mysql -u root -p

# Check tables:
USE geoentulho;
SHOW TABLES;
DESCRIBE Users;
SELECT * FROM Users;
```

---

## 📊 Development Checklist

### Before Commit
- [ ] Run `npm run build` in frontend (no errors)
- [ ] Run `dotnet build` in backend (no errors)
- [ ] Test login/register flow locally
- [ ] Check browser console for warnings
- [ ] Verify no hardcoded secrets in code
- [ ] Update PROGRESS.md with changes

### Code Style
- **Frontend**: 
  - Use functional components with hooks
  - Use camelCase for variables/functions
  - Use PascalCase for components
  - Comments for complex logic

- **Backend**:
  - Use PascalCase for class/method names
  - Use camelCase for properties
  - Follow C# naming conventions
  - XML comments on public methods

---

## 🚢 Deployment (Next Phase)

### Backend: Railway
1. Connect GitHub repository
2. Create MySQL add-on
3. Set environment variables
4. Deploy on push
5. Get production API URL

### Frontend: Vercel
1. Connect GitHub repository
2. Set environment variable: `VITE_API_URL={railway-backend-url}`
3. Deploy on push
4. Get production domain

---

## 📚 Additional Resources

- [React Documentation](https://react.dev)
- [React Router Guide](https://reactrouter.com)
- [.NET 8 Docs](https://learn.microsoft.com/dotnet/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [JWT Standard](https://tools.ietf.org/html/rfc7519)
- [MySQL Documentation](https://dev.mysql.com/doc/)

---

## 📞 Support & Questions

For issues, clarifications, or feature requests, refer to:
- PROGRESS.md - Current development status
- Code comments - Implementation details
- API Documentation - Swagger UI at `/swagger`

---

## ⚖️ License & Credits

**GeoEntulho** - Smart Cities Waste Management MVP
- Aligned with UN Sustainable Development Goals
- Open ecosystem for sustainable cities
- Built with ❤️ for a better planet

---

**Last Updated**: April 13, 2026
**Status**: Ready for development (Auth endpoints next)
**Estimated MVP Launch**: April 30, 2026
