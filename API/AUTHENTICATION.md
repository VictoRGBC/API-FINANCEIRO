# 🔐 Autenticação JWT - Financial Manager API

## 📋 Visão Geral

A API agora possui autenticação completa usando **JWT (JSON Web Tokens)** com BCrypt para hash de senhas.

## 🏗️ Arquitetura Implementada

### Componentes Criados

#### **Domain Layer**
- ✅ `User` - Entidade de usuário
- ✅ `IUserRepository` - Interface do repositório
- ✅ `UnauthorizedException` - Exceção customizada

#### **Application Layer**
- ✅ `AuthService` - Serviço de autenticação
- ✅ `ITokenService` - Interface do serviço de token
- ✅ `RegisterRequest` / `LoginRequest` / `AuthResponse` - DTOs
- ✅ `RegisterRequestValidator` / `LoginRequestValidator` - Validadores FluentValidation

#### **Infrastructure Layer**
- ✅ `UserRepository` - Implementação do repositório
- ✅ `UserConfiguration` - Configuração EF Core
- ✅ Migration: `AddUserAuthentication`

#### **API Layer**
- ✅ `AuthController` - Endpoints de autenticação
- ✅ `TokenService` - Geração de tokens JWT
- ✅ Middleware de autenticação JWT configurado
- ✅ Swagger com suporte a Bearer Token

---

## 🚀 Como Usar

### **1. Aplicar a Migration**

Antes de executar a API, aplique a migration para criar a tabela `Users`:

```bash
dotnet ef database update --project FinancialManager.Infrastructure --startup-project FinancialManager.API
```

### **2. Configuração (appsettings.json)**

As configurações JWT estão em `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-change-this-in-production-minimum-32-characters",
    "Issuer": "FinancialManagerAPI",
    "Audience": "FinancialManagerClient",
    "ExpirationHours": "8"
  }
}
```

⚠️ **IMPORTANTE**: Em produção, use **Azure Key Vault** ou **User Secrets** para armazenar a `SecretKey`.

---

## 📝 Endpoints da API

### **POST /api/auth/register**
Registra um novo usuário.

**Request:**
```json
{
  "name": "João Silva",
  "email": "joao@email.com",
  "password": "senha123"
}
```

**Response (201 Created):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "joao@email.com",
  "name": "João Silva",
  "expiresAt": "2024-01-15T10:30:00Z"
}
```

---

### **POST /api/auth/login**
Autentica um usuário existente.

**Request:**
```json
{
  "email": "joao@email.com",
  "password": "senha123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "joao@email.com",
  "name": "João Silva",
  "expiresAt": "2024-01-15T10:30:00Z"
}
```

---

## 🔒 Endpoints Protegidos

Todos os controllers agora exigem autenticação:
- ✅ `/api/account` - Gerenciamento de contas
- ✅ `/api/transaction` - Gerenciamento de transações
- ✅ `/api/transfer` - Transferências
- ✅ `/api/category` - Categorias

### **Como Acessar Endpoints Protegidos**

#### **Via Swagger**
1. Clique no botão **"Authorize"** (🔓)
2. Digite: `Bearer {seu-token-aqui}`
3. Clique em **"Authorize"**
4. Faça requisições normalmente

#### **Via Postman/Insomnia**
Adicione o header:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### **Via cURL**
```bash
curl -X GET "https://localhost:5001/api/account" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

## 🔍 Estrutura do Token JWT

O token contém os seguintes claims:

```json
{
  "sub": "user-id-guid",
  "email": "joao@email.com",
  "name": "João Silva",
  "jti": "unique-token-id",
  "userId": "user-id-guid",
  "exp": 1705318200,
  "iss": "FinancialManagerAPI",
  "aud": "FinancialManagerClient"
}
```

### **Acessando Informações do Usuário Autenticado**

No controller, você pode acessar:

```csharp
[HttpGet]
[Authorize]
public IActionResult GetCurrentUser()
{
    var userId = User.FindFirst("userId")?.Value;
    var email = User.FindFirst(ClaimTypes.Email)?.Value;
    var name = User.FindFirst(ClaimTypes.Name)?.Value;
    
    return Ok(new { userId, email, name });
}
```

---

## 🛡️ Segurança Implementada

### ✅ **O que foi implementado:**
- Hash de senha com BCrypt (salt automático)
- Tokens JWT com assinatura HMAC SHA-256
- Validação de issuer, audience e lifetime
- Proteção contra timing attacks (validação de senha)
- ClockSkew zerado (tokens expiram exatamente no tempo configurado)
- Middleware global de tratamento de exceções (incluindo 401)

### ⚠️ **Próximos Passos de Segurança:**
1. **Refresh Tokens** - Para renovar tokens expirados
2. **Rate Limiting** - Limitar tentativas de login
3. **Email Verification** - Confirmar email após registro
4. **Password Reset** - Recuperação de senha
5. **Two-Factor Authentication (2FA)** - Autenticação em duas etapas
6. **HTTPS Only** - Forçar HTTPS em produção
7. **CORS Restritivo** - Substituir "*" por domínios específicos

---

## 🧪 Testando a Autenticação

### **Fluxo Completo de Teste**

```bash
# 1. Registrar usuário
POST /api/auth/register
{
  "name": "Test User",
  "email": "test@email.com",
  "password": "Test123"
}

# 2. Fazer login
POST /api/auth/login
{
  "email": "test@email.com",
  "password": "Test123"
}

# 3. Copiar o token do response

# 4. Acessar endpoint protegido
GET /api/account
Header: Authorization: Bearer {token}
```

---

## ❌ Erros Comuns

### **401 Unauthorized**
- Token ausente ou inválido
- Token expirado
- Formato incorreto do header (deve ser `Bearer {token}`)

### **400 Bad Request**
- Email já cadastrado
- Dados de validação inválidos (senha curta, email inválido)

### **UnauthorizedException**
- Credenciais inválidas
- Usuário inativo

---

## 📦 Pacotes NuGet Adicionados

```xml
<!-- API Project -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.3.1" />

<!-- Application Project -->
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
```

---

## 🎯 Boas Práticas Implementadas

✅ Separação de responsabilidades (Clean Architecture)  
✅ Validação com FluentValidation  
✅ Exceções customizadas  
✅ Repository Pattern  
✅ Unit of Work Pattern  
✅ DTOs para requests/responses  
✅ Logging estruturado  
✅ Documentação Swagger com autenticação  
✅ Configuração via appsettings.json  

---

## 🔄 Próximas Melhorias

1. **Refresh Tokens** - Implementar renovação de tokens
2. **User Roles** - Adicionar autorização baseada em perfis (Admin, User)
3. **Email Service** - Envio de emails de confirmação
4. **Audit Log** - Registrar ações dos usuários
5. **Account Management** - Endpoints para atualizar perfil, mudar senha, etc.

---

## 📚 Referências

- [JWT.io](https://jwt.io/) - Debug de tokens JWT
- [BCrypt Docs](https://github.com/BcryptNet/bcrypt.net) - Documentação do BCrypt.Net
- [Microsoft Identity Docs](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/) - Autenticação no ASP.NET Core

---

**✅ Autenticação JWT implementada com sucesso!**  
Agora sua API está protegida e pronta para uso em produção (após configurar secrets management).
