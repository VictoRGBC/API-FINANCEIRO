# 🚀 Quick Start - JWT Authentication

## Passo a Passo para Configurar

### 1️⃣ **Aplicar Migration**
```bash
dotnet ef database update --project FinancialManager.Infrastructure --startup-project FinancialManager.API
```

### 2️⃣ **Executar a API**
```bash
dotnet run --project FinancialManager.API
```

### 3️⃣ **Acessar Swagger**
```
https://localhost:5001/swagger
```

### 4️⃣ **Testar no Swagger**

#### **A. Registrar um Usuário**
1. Expandir `POST /api/auth/register`
2. Clicar em "Try it out"
3. Inserir:
```json
{
  "name": "Admin User",
  "email": "admin@financeiro.com",
  "password": "Admin123"
}
```
4. Execute
5. **Copiar o `token` do response**

#### **B. Autorizar no Swagger**
1. Clicar no botão **"Authorize"** 🔓 (canto superior direito)
2. Digitar: `Bearer {seu-token-aqui}`
3. Clicar em **"Authorize"**
4. Clicar em **"Close"**

#### **C. Testar Endpoint Protegido**
1. Expandir `GET /api/account`
2. Clicar em "Try it out"
3. Execute
4. ✅ Deve retornar 200 OK

---

## 📝 Exemplo de Fluxo Completo

```bash
# 1. Registrar
curl -X POST "https://localhost:5001/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao@email.com",
    "password": "Senha123"
  }'

# Response:
# {
#   "token": "eyJhbGc...",
#   "email": "joao@email.com",
#   "name": "João Silva",
#   "expiresAt": "2024-01-15T18:00:00Z"
# }

# 2. Usar o token para acessar endpoints
curl -X GET "https://localhost:5001/api/account" \
  -H "Authorization: Bearer eyJhbGc..."
```

---

## ⚙️ Configuração de Desenvolvimento

### **User Secrets (Recomendado para Dev)**

```bash
# Inicializar user secrets
dotnet user-secrets init --project FinancialManager.API

# Adicionar chave JWT
dotnet user-secrets set "JwtSettings:SecretKey" "my-super-secret-development-key-with-at-least-32-characters" --project FinancialManager.API
```

### **appsettings.Development.json**
```json
{
  "JwtSettings": {
    "SecretKey": "development-secret-key-change-in-production-minimum-32-characters-required",
    "Issuer": "FinancialManagerAPI",
    "Audience": "FinancialManagerClient",
    "ExpirationHours": "8"
  }
}
```

---

## 🐛 Troubleshooting

### ❌ "Connection string not configured"
**Solução:** Verificar `appsettings.json` → ConnectionStrings → DefaultConnection

### ❌ "JWT SecretKey não foi configurada"
**Solução:** Adicionar JwtSettings no appsettings.json (já está configurado)

### ❌ "401 Unauthorized" ao acessar endpoints
**Solução:** 
1. Fazer login em `/api/auth/login`
2. Copiar o token do response
3. Adicionar no header: `Authorization: Bearer {token}`

### ❌ "Email já está cadastrado"
**Solução:** Usar outro email ou fazer login com o existente

---

## 📊 Estrutura do Banco de Dados

Após a migration, a tabela `Users` terá:

| Campo         | Tipo          | Descrição                      |
|---------------|---------------|--------------------------------|
| Id            | uniqueidentifier | Primary Key (GUID)          |
| Name          | nvarchar(100) | Nome do usuário                |
| Email         | nvarchar(150) | Email (unique)                 |
| PasswordHash  | nvarchar(255) | Hash BCrypt da senha           |
| CreatedAt     | datetime2     | Data de criação                |
| LastLoginAt   | datetime2     | Último login (nullable)        |
| IsActive      | bit           | Status do usuário              |

**Relacionamento:**
- `Users` 1:N `Accounts` (um usuário pode ter várias contas)

---

## ✅ Checklist de Implementação

- [x] Entidade User criada
- [x] UserRepository implementado
- [x] AuthService com Register e Login
- [x] TokenService para gerar JWT
- [x] AuthController com endpoints
- [x] Validadores FluentValidation
- [x] Migration AddUserAuthentication
- [x] Middleware JWT configurado
- [x] Controllers protegidos com [Authorize]
- [x] Swagger com Bearer Token
- [x] GlobalExceptionHandler atualizado
- [x] Documentação completa

---

## 🎯 Próximos Passos

1. ✅ Testar todos os endpoints
2. ✅ Verificar logs de autenticação
3. 🔄 Implementar Refresh Tokens
4. 🔄 Adicionar User Roles (Admin, User)
5. 🔄 Implementar Email Verification
6. 🔄 Rate Limiting
7. 🔄 Password Reset Flow

---

**Pronto! Sua API agora está com autenticação JWT funcionando! 🎉**
