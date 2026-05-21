# 🚀 GeoEntulho - Firebase Integration Phase (Etapa 3)

**Data**: 14 de Abril de 2026 (Etapa 3 - Firebase Backend)
**Status**: Fase 3 - Firebase Integration (🔄 IN PROGRESS)
**Prazo**: Deploy Firebase + Railway backend  
**Frontend Status**: 🟢 DEPLOYED ON VERCEL (https://geo-entulho-scq0bw8xs-pedrowans-projects.vercel.app)
**Backend Status**: 🟡 Code updated for Firebase (awaiting credentials & deployment)

---

## ✅ O Que Foi Completado

### FASE 1: Backend Foundation (100% ✅)
- ✅ Projeto .NET 8 inicializado
- ✅ 5 Domain Models criados (User, Company, CollectionPoint, Ticket, TicketUpdate)
- ✅ ApplicationDbContext configurado
- ✅ Program.cs totalmente refatorado para Firebase
- ✅ JWT Authentication com claims (Email, Name, Type)
- ✅ CORS configurado para frontend em produção

### FASE 2: Frontend Integration (100% ✅)
- ✅ React/Vite projeto funcional
- ✅ React Router v6 com proteção de rotas
- ✅ AuthContext com useAuth hook
- ✅ Axios com JWT interceptors automáticos
- ✅ Login, Register, Home pages
- ✅ **Deployed to Vercel** (auto-deploy from GitHub)

### FASE 3: Firebase Integration (🔄 IN PROGRESS)
- ✅ FirebaseService.cs criado (IFirebaseService interface)
- ✅ AuthController reescrito para usar Firebase Auth + Firestore
- ✅ TicketsController reescrito para usar Firebase
- ✅ Program.cs atualizado: Firebase registration + JWT config
- ✅ Todas as mudanças committed e pushed para GitHub main
- 🟡 **AWAITING**: Firebase project credentials (FIREBASE_PROJECT_ID, FIREBASE_API_KEY)
- 🟡 **AWAITING**: Railway environment variables setup
- 🟡 **AWAITING**: Backend deployment to Railway

---

## � PRÓXIMAS TAREFAS (Sequência para Deploy)

### **1️⃣ CRÍTICO: Criar/Configurar Firebase Project (30 min)**

**Option A: Usar um Firebase Project Existente**
- [ ] Acessar https://console.firebase.google.com
- [ ] Selecionar projeto existente
- [ ] Anotar FIREBASE_PROJECT_ID

**Option B: Criar Novo Firebase Project**
- [ ] Ir em https://console.firebase.google.com
- [ ] Click "+ Adicionar Projeto"
- [ ] Nome: "GeoEntulho"
- [ ] Aceitar termos, criar projeto
- [ ] Anotar `FIREBASE_PROJECT_ID`

**Configure Firebase Authentication:**
- [ ] No Firebase Console → Authentication
- [ ] Click "Começar"
- [ ] Enable "Email/Password" provider
- [ ] Copiar sua API Key (ou ir em Settings → Project Settings → Web API Key)
- [ ] Anotar `FIREBASE_API_KEY`

**Create Firestore Database:**
- [ ] Firebase Console → Firestore Database → Criar Database
- [ ] Modo: Production (regras protegidas)
- [ ] Região: us-central1 (ou próxima)
- [ ] **IMPORTANTE**: Criar duas collections manualmente ou deixar que a app crie:
  - `users` (documents: user IDs)
  - `tickets` (documents: ticket IDs)

---

### **2️⃣ CRÍTICO: Railway Environment Variables (15 min)**

**Acessar Railway Dashboard:**
- [ ] https://railway.app/dashboard
- [ ] Selecionar projeto "GeoEntulho"
- [ ] Click em "Variables" tab
- [ ] Adicionar as 4 variáveis abaixo:

| Variable Name | Value | Source |
|---------------|-------|--------|
| `FIREBASE_PROJECT_ID` | (seu project ID) | Firebase Console |
| `FIREBASE_API_KEY` | (sua API key) | Firebase Settings |
| `JWT_SECRET` | (gerar: openssl rand -hex 32) | Gerar novo |
| `FRONTEND_URL` | https://geo-entulho-scq0bw8xs-pedrowans-projects.vercel.app | Vercel URL |

**Após adicionar variáveis:**
- [ ] Click "Save changes"
- [ ] Click botão "Redeploy" para reiniciar com novas variáveis

---

### **3️⃣ Verificar Deployment (10 min)**

**Railway Console:**
- [ ] Abrir Logs tab
- [ ] Procurar por: `[GeoEntulho] ✓ Firebase configured`
- [ ] Se aparecer, Firebase está OK ✓
- [ ] Se não, procurar por erros

**Testar Endpoints (Postman ou Thunder Client):**
```
POST https://<railway-backend-url>/api/auth/register
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "SecurePassword123",
  "name": "Test User",
  "type": "citizen"
}
```

---

## 📊 Current Code Status

### Backend Files Updated
| File | Changes | Status |
|------|---------|--------|
| [FirebaseService.cs](backend/GeoEntulho.API/Services/FirebaseService.cs) | ✅ Created - 7 core methods | ✅ Committed |
| [AuthController.cs](backend/GeoEntulho.API/Controllers/AuthController.cs) | ✅ Rewritten for Firebase | ✅ Committed |
| [TicketsController.cs](backend/GeoEntulho.API/Controllers/TicketsController.cs) | ✅ Rewritten for Firebase | ✅ Committed |
| [Program.cs](backend/GeoEntulho.API/Program.cs) | ✅ Removed MySQL, added Firebase | ✅ Committed |
| GeoEntulho.API.csproj | ✅ FirebaseAdmin v2.4.0 added | ✅ Done |

### Frontend Status (No Changes)
- ✅ Already deployed to Vercel
- ✅ Ready to connect to Firebase backend
- ℹ️ Currently pointing to old backend (will update when Railway deployed)

---

## 🔄 Architecture Overview (Firebase)

```
┌─────────────────┐
│  React Frontend │ (Vercel)
│   (deployed)    │
└────────┬────────┘
         │ (HTTPS)
         ↓
┌─────────────────────────────┐
│  ASP.NET Core Backend       │ (Railway)
│  - Program.cs: IFirebaseService registered
│  - AuthController: Firebase Auth + JWT
│  - TicketsController: Firestore CRUD
└────────┬────────────────────┘
         │ (gRPC/REST)
         ↓
┌──────────────────────────────┐
│  Google Cloud Firestore      │
│  - Collection: users         │
│  - Collection: tickets       │
│  - Real-time sync enabled    │
└──────────────────────────────┘
```

---

## 🎯 Next Phase: Local Testing (Optional)

Para testar localmente ANTES de deploy:

```bash
# Terminal 1: Backend
cd D:\GeoEntulho\backend\GeoEntulho.API
$env:FIREBASE_PROJECT_ID="your-project-id"
$env:FIREBASE_API_KEY="your-api-key"
$env:JWT_SECRET="$(openssl rand -hex 32)"
$env:FRONTEND_URL="http://localhost:5173"
dotnet run

# Terminal 2: Frontend
cd D:\GeoEntulho\frontend\geoentulho-web
npm run dev
```

Então:
1. Abrir http://localhost:5173
2. Registrar um novo usuário
3. Verificar se o usuário foi criado no Firestore
4. Fazer login
5. Testar criar um ticket

```
D:\GeoEntulho\
├── backend\
│   └── GeoEntulho.API\
│       ├── Models\ ✅
│       │   ├── User.cs
│       │   ├── Company.cs
│       │   ├── CollectionPoint.cs
│       │   ├── Ticket.cs
│       │   └── TicketUpdate.cs
│       ├── Data\ ✅
│       │   └── ApplicationDbContext.cs
│       ├── appsettings.json ✅
│       ├── Program.cs ✅
│       └── Migrations\ ✅
│
├── frontend\
│   └── geoentulho-web\
│       ├── src\
│       │   ├── App.jsx ✅ (Router + ProtectedRoute)
│       │   ├── main.jsx ✅ (Entry point)
│       │   ├── pages\ ✅
│       │   │   ├── Login.jsx (Página /login)
│       │   │   ├── Register.jsx (Página /register)
│       │   │   └── Home.jsx (Página / protegida)
│       │   ├── services\ ✅
│       │   │   ├── api.js (Axios + JWT)
│       │   │   └── authService.js (Register/Login/Logout)
│       │   ├── context\ ✅
│       │   │   └── AuthContext.jsx (useAuth hook)
│       │   └── styles\ ✅
│       │       ├── global.css
│       │       ├── auth.css
│       │       └── home.css
│       ├── .env ✅ (VITE_API_URL=http://localhost:5242)
│       ├── .env.example ✅
│       ├── package.json ✅
│       └── vite.config.js ✅
│
└── PROGRESS.md (este arquivo)
```

---

## 🔗 Fluxo de Autenticação Implementado

### **Frontend → Backend Integration**

1. **Registro** (`/register`)
   - User submete: email, senha, nome, tipo (citizen/company)
   - React (Register.jsx) → authService.register() 
   - authService → api.js (axios POST)
   - Servidor HTTP → Backend API
   - Backend retorna: { success, message, token, user }
   - Frontend redireciona para /login

2. **Login** (`/login`)
   - User submete: email, senha
   - React (Login.jsx) → authService.login()
   - Backend retorna JWT token
   - Token armazenado em localStorage
   - Redux Context atualiza: user + isAuthenticated = true
   - Frontend redireciona para / (Home)

3. **Protected Routes**
   - App.jsx envolve Home com `<ProtectedRoute>`
   - Se não autenticado → redireciona para /login
   - useAuth() fornece: user, isAuthenticated(), logout()

4. **Logout** (`/` - Home page)
   - Click botão logout
   - authService.logout()
   - localStorage.removeItem('token')
   - AuthContext limpa state
   - Redireciona para /login

5. **Auto-logout (401)** 
   - API returns 401 Unauthorized
   - axios response interceptor detecta
   - redirectTo: /login automáticamente

---

## 🛠️ Tecnologias Implementadas

**Frontend Stack:**
- React 19.2.4 com Hooks
- React Router v6 (routing + navigation)
- Vite 8.0 (ultra-fast build 312ms)
- Axios (HTTP client + JWT interceptors)
- React Context API (auth state management)
- Zustand (instalado, pronto para features)
- Leaflet + react-leaflet (mapas)
- CSS3 (responsive design)

**Backend Stack:**
- .NET 8 SDK
- ASP.NET Core Web API
- Entity Framework Core 8.0
- Pomelo EF MySQL Provider 8.0.0
- JWT Authentication (System.IdentityModel.Tokens.Jwt 8.17.0)
- MySQL 9.6

---

## 📋 Próximas Tarefas (Imediatamente)

### **CRÍTICO: AuthController + DTOs (40 min)**

**Arquivo**: `D:\GeoEntulho\backend\GeoEntulho.API\DTOs\AuthDto.cs`
```csharp
public class RegisterDto {
  [Required, EmailAddress]
  public string Email { get; set; }
  [Required, MinLength(8)]
  public string Password { get; set; }
  [Required]
  public string Name { get; set; }
  [Required]
  public string Type { get; set; }  // "citizen" ou "company"
}

public class LoginDto {
  [Required, EmailAddress]
  public string Email { get; set; }
  [Required]
  public string Password { get; set; }
}

public class AuthResponseDto {
  public bool Success { get; set; }
  public string Message { get; set; }
  public string Token { get; set; }
  public UserDto User { get; set; }
}

public class UserDto {
  public int Id { get; set; }
  public string Email { get; set; }
  public string Name { get; set; }
  public string Type { get; set; }
}
```

**Arquivo**: `D:\GeoEntulho\backend\GeoEntulho.API\Services\AuthService.cs`
- ✅ Implement: Register(RegisterDto)
- ✅ Implement: Login(LoginDto)
- ✅ Implement: GenerateJwtToken(User)
- ✅ Use BCrypt.Net-Next para password hashing

**Arquivo**: `D:\GeoEntulho\backend\GeoEntulho.API\Controllers\AuthController.cs`
```
POST /api/auth/register      → Register new user
POST /api/auth/login         → Generate JWT token
GET /api/auth/me (protected) → Current user info
```

### **Test Full Loop (20 min)**
1. [ ] npm run dev (Frontend dev server)
2. [ ] dotnet run (Backend)
3. [ ] Register form test → Save to database
4. [ ] Login form test → JWT token generated
5. [ ] Home.jsx loads → Showing user info
6. [ ] Logout button → Token cleared

---

## ✅ Validation Checklist Before Next Phase

### Backend
- [ ] DTOs criados com validação
- [ ] AuthService implementado
- [ ] AuthController endpoints funcionando
- [ ] Swagger mostra os 3 endpoints
- [ ] Postman/Thunder Client pode testar

### Frontend
- [ ] npm run dev inicia sem erros
- [ ] Login page renderiza
- [ ] Register page renderiza
- [ ] Home page protegida (redirect se não auth)
- [ ] Console sem erros (F12)

### Integration
- [ ] Register flow completo (React → Database)
- [ ] Login flow completo (React → JWT token)
- [ ] JWT token salvo em localStorage
- [ ] Protected route funciona
- [ ] Logout limpa dados

---

## 📊 Project Status Overview

| Aspecto | Status | % Completo |
|---------|--------|-----------|
| Backend Setup | ✅ Concluído | 100% |
| Database | ✅ Concluído | 100% |
| Frontend Structure | ✅ Concluído | 100% |
| Authentication UI | ✅ Concluído | 100% |
| Auth Endpoints | 🟡 In Progress | 0% |
| Tickets Management | ⏳ Todo | 0% |
| Company Dashboard | ⏳ Todo | 0% |
| Maps Integration | ⏳ Todo | 0% |
| Deployment Setup | ⏳ Todo | 0% |
| **TOTAL PROJECT** | **50%** | **50%** |

---

## 🚀 How to Run

### Terminal 1: Backend
```bash
cd D:\GeoEntulho\backend\GeoEntulho.API
dotnet run
# Listening on http://localhost:5242
# Swagger: http://localhost:5242/swagger
```

### Terminal 2: Frontend
```bash
cd D:\GeoEntulho\frontend\geoentulho-web
npm run dev
# Listening on http://localhost:5173
# Auto-refresh on file changes
```

### Test in Browser
1. Open http://localhost:5173/
2. Redirects to /login (protected route)
3. Click "Registre-se aqui"
4. Fill form: email, password, name, type
5. Click Registrar → Goes to /login
6. Fill login: email, password
7. Click Entrar → Goes to / (Home dashboard)
8. See user info and logout button

---

## 🔒 Security Configuration

**JWT (appsettings.json)**:
- Key: 32+ random characters
- Duration: 1440 minutes (24h)
- Algorithm: HS256
- Issuer: geoentulho-api
- Audience: geoentulho-web

**CORS (Program.cs)**:
- Allowed hosts: localhost:5173, localhost:3000
- Methods: GET, POST, PUT, DELETE
- Headers: Content-Type, Authorization
- Credentials: Not allowed (token-based)

**Passwords (Backend - TODO)**:
- Hash with BCrypt
- Minimum 8 characters
- Never log/return plaintext

---

## 📱 Responsive Design

**Breakpoints (CSS)**:
- Mobile: 0-600px (Login/Register forms stack)
- Tablet: 600-1200px (Dashboard 2-col grid)
- Desktop: 1200px+ (Full 3-col grid)

**Colors**:
- Primary: #2D7A5B (green - nature/environment)
- Secondary: #3B9B6F (lighter green)
- Accent: #4A7C9E (company blue)
- Error: #ff6b6b (red)

---

## 📚 Component Documentation

### `AuthContext.jsx`
- **Export**: AuthProvider, useAuth
- **useAuth hook returns**:
  - `user`: { id, email, name, type }
  - `isAuthenticated()`: boolean
  - `login(email, password)`: Promise
  - `register(email, password, name, type)`: Promise
  - `logout()`: void
  - `loading`: boolean

### `api.js`
- **Base URL**: `${import.meta.env.VITE_API_URL}`
- **Request interceptor**: Adds Authorization header
- **Response interceptor**: 401 → redirect to /login

### `authService.js`
- **Methods**:
  - `register(email, password, name, type)`: POST /api/auth/register
  - `login(email, password)`: POST /api/auth/login
  - `logout()`: Clear localStorage
  - `getCurrentUser()`: Get from localStorage
  - `isAuthenticated()`: Check token existence
  - `setToken(token)`: Store in localStorage
  - `getToken()`: Retrieve token

---

## 🎯 Milestone Timeline

| Data | Milestone | Status |
|------|-----------|--------|
| Apr 10 | Proposal validation | ✅ Done |
| Apr 13 | Backend foundation | ✅ Done |
| Apr 13 | Frontend integration | ✅ Done |
| Apr 13-14 | Auth endpoints | 🟡 Today |
| Apr 14-15 | Tickets CRUD | ⏳ Next |
| Apr 16 | Maps integration | ⏳ Next |
| Apr 17-25 | Company features | ⏳ Next |
| Apr 26-27 | Deployment setup | ⏳ Next |
| Apr 28-29 | Production testing | ⏳ Next |
| Apr 30 | MVP Launch 🎉 | ⏳ Final |

---

**Last Updated**: 13 Abril 2026, 23:50 UTC
**Next Milestone**: AuthController + DTOs Implementation
**Estimated Duration**: 40 minutes
