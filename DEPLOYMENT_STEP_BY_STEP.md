# 🚀 GUIA PASSO A PASSO - DEPLOYMENT GEOENTULHO

## Link do Repositório
**GitHub:** https://github.com/PedroWan/GeoEntulho

---

## 📱 PARTE 1: Deploy do Frontend (Vercel)

### Passo 1: Criar Conta Vercel
1. Ir em https://vercel.com/signup
2. Clicar "Continue with GitHub"
3. Autorizar Vercel a acessar suas repos GitHub

### Passo 2: Importar Projeto
1. No dashboard Vercel, clicar **"Add New... → Project"**
2. Clicar em **"Import Git Repository"**
3. Procurar por `GeoEntulho` e clicar "Import"

### Passo 3: Configurar Projeto
1. **Framework Preset:** Deixar em branco (Vite será detectado)
2. **Root Directory:** 
   - Clicar "Edit"
   - Mudar para: `frontend/geoentulho-web`
   - Clicar "Continue"

### Passo 4: Variáveis de Ambiente
Na seção "Environment Variables", adicionar:
```
VITE_API_URL = https://seu-backend-railway.up.railway.app
```
(Você pegará a URL do Railway depois)

### Passo 5: Deploy
1. Clicar **"Deploy"**
2. Esperar build terminar (±2-3 minutos)
3. ✅ Seu frontend estará em: `https://geoentulho.vercel.app` (ou similar)

---

## 🗄️ PARTE 2: Deploy do Backend + Database (Railway)

### Passo 1: Criar Conta Railway
1. Ir em https://railway.app
2. Clicar "Dashboard"
3. Clicar "Create New Project" ou "New"

### Passo 2: Conectar GitHub
1. Clicar **"Deploy from GitHub"**
2. Clicar "Configure GitHub App"
3. Autorizar Railway a acessar suas repos
4. Selecionar `GeoEntulho` repository

### Passo 3: Configurar Backend Service
1. Selecionar `GeoEntulho` repository
2. Railway detectará e criará um serviço
3. Clicar na engrenagem ⚙️ (Settings) do serviço

### Passo 4: Configurar Root Directory
Em Settings:
- **Root Directory:** `backend/GeoEntulho.API`
- **Framework:** .NET
- Salvar

### Passo 5: Adicionar Banco de Dados MySQL
1. No dashboard do projeto, clicar **"+ New"**
2. Clicar **"Database"** → **"MySQL"**
3. Railway criará uma instância MySQL automaticamente
4. As variables aparecerão em "Variables"

### Passo 6: Configurar Variáveis de Ambiente
1. Ir para a aba **"Variables"** do projeto
2. Adicionar as seguintes variáveis:

```
# Backend
ASPNETCORE_ENVIRONMENT = Production
JWT_SECRET = gera-uma-chave-aleatoria-super-segura-aqui

# Vai estar auto-gerado pelo Railway
DATABASE_URL = mysql://root:password@mysql.railway.internal:3306/railway
```

3. Clicar "Save"

### Passo 7: Deploy
1. Railway fará deploy automático
2. Você pode monitorar em "Deployments"
3. Copiar a **URL do Backend** (algo como: `https://seu-backend-railway.up.railway.app`)

---

## 🔗 PARTE 3: Atualizar Frontend com URL do Backend

1. Voltar para **Vercel Dashboard**
2. Selecionar projeto `GeoEntulho`
3. Ir para **"Settings"** → **"Environment Variables"**
4. Editar `VITE_API_URL`:
   ```
   VITE_API_URL = https://seu-backend-railway.up.railway.app
   ```
5. Salvar e fazer re-deploy:
   - Clicar **"Deployments"** → Clicar os três pontos **"..."**
   - Selecionar **"Redeploy"**

---

## ✅ VERIFICAÇÃO FINAL

### Testar Frontend
- Abrir: https://seu-projeto.vercel.app
- Ver Landing page
- Registrar nova conta
- Criar ticket
- Verificar se salva

### Testar Backend API
- Abrir: https://seu-backend-railway.up.railway.app/swagger
- Ver documentação Swagger
- Testar endpoints manualmente

### Testar Banco de Dados
- Railway → Project → MySQL → Connect
- Usar ferramenta como DBeaver ou TablePlus
- Verificar tables: Users, Tickets, etc.

---

## 🐛 TROUBLESHOOTING

### Frontend não conecta com Backend
- **Erro:** "Failed to fetch from API"
- **Solução:** Verificar `VITE_API_URL` em Vercel Variables
- Fazer redeploy do Vercel

### Database connection failed
- **Erro:** "Unable to connect to database"
- **Solução:** Verificar Railway MySQL Variables
- Copiar correta a `DATABASE_URL`

### Swagger retorna 404
- **Erro:** "Cannot find /swagger"
- **Solução:** Backend está em produção, Swagger desabilitado
- Isso é normal, API ainda funciona

### Erro de CORS
- **Erro:** "Access to XMLHttpRequest blocked by CORS policy"
- **Solução:** Backend CORS configurado para aceitar qualquer origem em prod
- Se persistir, verificar Network tab no DevTools

---

## 📊 URLs Finais

| Componente | URL |
|---|---|
| Frontend | https://seu-projeto.vercel.app |
| Backend API | https://seu-backend-railway.up.railway.app/api |
| Swagger Docs | https://seu-backend-railway.up.railway.app/swagger (dev only) |

---

## 💰 Custo

- **Vercel Frontend:** ✅ GRÁTIS
- **Railway Backend + Database:** ✅ GRÁTIS ($5/mês de crédito)
- **TOTAL:** ✅ **$0/mês**

---

## 🚀 Próximas Etapas

1. **Domain Customizado:** Comprar domínio e apontar para Vercel/Railway
2. **SSL/TLS:** Automático no Vercel e Railway
3. **Monitoramento:** Configurar alertas em Railway
4. **Backups:** Configurar backups automáticos do MySQL

---

## 📞 Suporte

- **Vercel Docs:** https://vercel.com/docs
- **Railway Docs:** https://docs.railway.app
- **GitHub Issues:** https://github.com/PedroWan/GeoEntulho/issues
