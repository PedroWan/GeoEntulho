# 🚂 Railway - Configuração de Variáveis de Ambiente

## Variáveis Necessárias

Configure as seguintes variáveis no Railway Dashboard → GeoEntulho → Variables:

| Variável | Valor | Exemplo |
|----------|-------|---------|
| `JWT_SECRET` | Chave secreta JWT (32+ caracteres) | `abc123def456ghi789jkl012mno345pqr` |
| `DB_HOST` | Host do MySQL Railway | `railway.app` (ou IP fornecido) |
| `DB_PORT` | Porta do MySQL | `3306` |
| `DB_NAME` | Nome do banco de dados | `geoentulho` |
| `DB_USER` | Usuário do MySQL | `root` |
| `DB_PASSWORD` | Senha do MySQL | `sua-senha-segura-aqui` |

---

## 📋 Como Obter as Credenciais do MySQL

### Se você JÁ adicionou MySQL no Railway:

1. Vai ao **Railway Dashboard**
2. Clica no seu projeto **GeoEntulho**
3. Na esquerda, você verá **"MySQL"** (ou similar)
4. Clica em **"MySQL"**
5. Vai na aba **"Data"**
6. Verá um bloco com as credenciais:
   ```
   MYSQLHOST=...
   MYSQLPORT=3306
   MYSQLDATABASE=railway
   MYSQLUSER=root
   MYSQLPASSWORD=...
   ```

### Se você NÃO adicionou MySQL ainda:

1. Vai ao **Railway Dashboard**
2. Clica em **"+ Create"**
3. Seleciona **"Add MySQL"**
4. Aguarda provisionar (leva ~1 minuto)
5. Depois segue os passos acima

---

## 🔑 Como Gerar uma JWT_SECRET Forte

Execute no terminal:

```bash
node -e "console.log(require('crypto').randomBytes(32).toString('hex'))"
```

Ou use um gerador online: https://www.random.org/strings/

Precisa ter **mínimo 32 caracteres**.

---

## ✅ Checklist de Configuração

- [ ] MySQL foi adicionado no Railway
- [ ] Obtive as 6 variáveis do MySQL
- [ ] Gerei uma JWT_SECRET forte
- [ ] Adicionei as 6 variáveis no Railway → Variables
- [ ] Cliquei em **"Redeploy"** para aplicar as mudanças
- [ ] Backend está rodando sem erros

---

## 🆘 Troubleshooting

**Erro: "Connection refused"**
- Verifique se DB_HOST, DB_PORT, DB_USER e DB_PASSWORD estão corretos
- Confirme que o MySQL foi provisionado no Railway

**Erro: "JWT Key not configured"**
- Você colocou JWT_SECRET na lista de variables?
- Espere alguns segundos depois de salvar as variables antes de fazer redeploy

**Backend rodando mas 404 em /api/...**
- Backend está rodando, mas frontend não sabe onde conectar
- Configure `VITE_API_URL` no Vercel com a URL do Railway backend
