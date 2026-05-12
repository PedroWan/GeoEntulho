# 🚀 Instruções de Deployment - GeoEntulho

## Deploy em Produção (100% Gratuito)

### Frontend (React/Vite) - Vercel

1. **Conectar GitHub ao Vercel:**
   - Ir em [vercel.com](https://vercel.com)
   - Clicar "New Project"
   - Conectar GitHub
   - Selecionar repositório `GeoEntulho`

2. **Configurar Root Directory:**
   - Root Directory: `frontend/geoentulho-web`

3. **Variáveis de Ambiente:**
   ```
   VITE_API_URL=https://seu-backend-railway.railway.app
   ```

4. **Deploy:**
   - Clicar "Deploy"
   - Vercel faz auto-deploy a cada push em `main`

### Backend (ASP.NET Core) - Railway

1. **Conectar GitHub ao Railway:**
   - Ir em [railway.app](https://railway.app)
   - Clicar "New Project" → "Deploy from GitHub"
   - Conectar GitHub
   - Selecionar repositório `GeoEntulho`

2. **Configurar:**
   - Service: `GeoEntulho API`
   - Root Directory: `backend/GeoEntulho.API`

3. **Variáveis de Ambiente:**
   ```
   ASPNETCORE_ENVIRONMENT=Production
   JWT_SECRET=gera_uma_chave_segura_aleatoria_aqui
   ConnectionStrings__DefaultConnection=mysql://user:password@host:port/geoentulho
   ```

   **Nota:** Railway fornece MySQL automaticamente

4. **Banco de Dados:**
   - Railway cria MySQL automaticamente
   - Variables aparecem em "Variables" aba
   - Usar `${{MySQL.DATABASE_URL}}` para connection string

5. **Deploy:**
   - Railway faz auto-deploy a cada push em `main`

## ⚠️ Antes de Fazer Push

1. Verificar `.env` não está no repositório (deve estar em `.gitignore`)
2. Certificar que `Dockerfile` ou `railway.toml` está no root
3. Frontend `.env.production` não deve ter dados sensíveis

## 🔗 URLs de Produção

- **Frontend:** `https://seu-dominio.vercel.app`
- **Backend API:** `https://seu-backend-railway.railway.app/api`
- **Swagger:** `https://seu-backend-railway.railway.app/swagger`

## 📊 Monitoramento

- **Vercel:** [vercel.com/dashboard](https://vercel.com/dashboard)
- **Railway:** [railway.app/dashboard](https://railway.app/dashboard)

## 🐛 Troubleshooting

### CORS errors
- Backend CORS deve incluir frontend URL
- Editar `Program.cs` → `AddCors()`

### Database connection failed
- Verificar connection string em Railway Variables
- Usar `${{MySQL.DATABASE_URL}}`

### Build failed
- Verificar logs no Vercel/Railway dashboard
- Certificar `appsettings.json` está configurado
- Node dependencies: rodar `npm install` localmente primeiro
