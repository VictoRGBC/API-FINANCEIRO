# 🎯 Exemplos Práticos - API Financial Manager com JWT

## 📋 Cenários de Uso Completos

### **Cenário 1: Novo Usuário (First-Time User)**

#### **Passo 1: Registrar**
```http
POST https://localhost:5001/api/auth/register
Content-Type: application/json

{
  "name": "Maria Silva",
  "email": "maria@email.com",
  "password": "Senha123"
}
```

**Response (201 Created):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI3NWM4ZWNhYS0zZjI5LTRkYmEtOGY1ZC0xYzQyNjBkZjU3NmEiLCJlbWFpbCI6Im1hcmlhQGVtYWlsLmNvbSIsIm5hbWUiOiJNYXJpYSBTaWx2YSIsImp0aSI6IjkxZjJlYmEzLTRhNzYtNGU4Yi05MmYzLTZjNTM0NWI4YzdjZCIsInVzZXJJZCI6Ijc1YzhlY2FhLTNmMjktNGRiYS04ZjVkLTFjNDI2MGRmNTc2YSIsImV4cCI6MTcwNTM0NjQwMCwiaXNzIjoiRmluYW5jaWFsTWFuYWdlckFQSSIsImF1ZCI6IkZpbmFuY2lhbE1hbmFnZXJDbGllbnQifQ.1234567890abcdef",
  "email": "maria@email.com",
  "name": "Maria Silva",
  "expiresAt": "2024-01-15T18:00:00Z"
}
```

#### **Passo 2: Criar Conta Bancária**
```http
POST https://localhost:5001/api/account
Content-Type: application/json
Authorization: Bearer eyJhbGc...

{
  "name": "Conta Corrente Santander",
  "type": "Corrente",
  "userId": "75c8ecaa-3f29-4dba-8f5d-1c4260df576a"
}
```

**Response (201 Created):**
```json
{
  "id": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
  "name": "Conta Corrente Santander",
  "type": "Corrente",
  "userId": "75c8ecaa-3f29-4dba-8f5d-1c4260df576a",
  "balance": 0,
  "createdAt": "2024-01-15T10:00:00Z"
}
```

#### **Passo 3: Criar Categorias**
```http
POST https://localhost:5001/api/category
Content-Type: application/json
Authorization: Bearer eyJhbGc...

{
  "name": "Salário",
  "type": "Receita"
}
```

```http
POST https://localhost:5001/api/category
Content-Type: application/json
Authorization: Bearer eyJhbGc...

{
  "name": "Alimentação",
  "type": "Despesa"
}
```

#### **Passo 4: Adicionar Receita**
```http
POST https://localhost:5001/api/transaction
Content-Type: application/json
Authorization: Bearer eyJhbGc...

{
  "accountId": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
  "categoryId": "{id-categoria-salario}",
  "type": "Receita",
  "amount": 5000.00,
  "description": "Salário Janeiro 2024",
  "date": "2024-01-05T00:00:00Z"
}
```

#### **Passo 5: Adicionar Despesa**
```http
POST https://localhost:5001/api/transaction
Content-Type: application/json
Authorization: Bearer eyJhbGc...

{
  "accountId": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
  "categoryId": "{id-categoria-alimentacao}",
  "type": "Despesa",
  "amount": 150.00,
  "description": "Supermercado",
  "date": "2024-01-10T00:00:00Z"
}
```

#### **Passo 6: Consultar Saldo**
```http
GET https://localhost:5001/api/account/a1b2c3d4-e5f6-7890-1234-567890abcdef
Authorization: Bearer eyJhbGc...
```

**Response:**
```json
{
  "id": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
  "name": "Conta Corrente Santander",
  "type": "Corrente",
  "balance": 4850.00,
  "transactionsCount": 2
}
```

---

### **Cenário 2: Usuário Existente (Returning User)**

#### **Login**
```http
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "email": "maria@email.com",
  "password": "Senha123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGc...[novo-token]",
  "email": "maria@email.com",
  "name": "Maria Silva",
  "expiresAt": "2024-01-15T18:00:00Z"
}
```

---

### **Cenário 3: Transferência Entre Contas**

#### **Passo 1: Criar Segunda Conta**
```http
POST https://localhost:5001/api/account
Content-Type: application/json
Authorization: Bearer eyJhbGc...

{
  "name": "Conta Poupança",
  "type": "Poupança",
  "userId": "75c8ecaa-3f29-4dba-8f5d-1c4260df576a"
}
```

#### **Passo 2: Fazer Transferência**
```http
POST https://localhost:5001/api/transfer
Content-Type: application/json
Authorization: Bearer eyJhbGc...

{
  "fromAccountId": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
  "toAccountId": "{id-conta-poupanca}",
  "amount": 1000.00,
  "description": "Transferência para poupança"
}
```

**Response (200 OK):**
```json
{
  "transferId": "xyz123...",
  "fromAccountId": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
  "toAccountId": "{id-conta-poupanca}",
  "amount": 1000.00,
  "description": "Transferência para poupança",
  "date": "2024-01-15T10:30:00Z",
  "fromAccountBalance": 3850.00,
  "toAccountBalance": 1000.00
}
```

---

### **Cenário 4: Relatórios e Consultas**

#### **Listar Todas as Transações de uma Conta**
```http
GET https://localhost:5001/api/transaction?accountId=a1b2c3d4-e5f6-7890-1234-567890abcdef
Authorization: Bearer eyJhbGc...
```

#### **Filtrar Transações por Período**
```http
GET https://localhost:5001/api/transaction?accountId=a1b2c3d4&startDate=2024-01-01&endDate=2024-01-31
Authorization: Bearer eyJhbGc...
```

#### **Listar Todas as Contas**
```http
GET https://localhost:5001/api/account
Authorization: Bearer eyJhbGc...
```

**Response:**
```json
{
  "items": [
    {
      "id": "a1b2c3d4-e5f6-7890-1234-567890abcdef",
      "name": "Conta Corrente Santander",
      "type": "Corrente",
      "balance": 3850.00
    },
    {
      "id": "xyz789...",
      "name": "Conta Poupança",
      "type": "Poupança",
      "balance": 1000.00
    }
  ],
  "totalBalance": 4850.00
}
```

#### **Listar Categorias**
```http
GET https://localhost:5001/api/category
Authorization: Bearer eyJhbGc...
```

---

## 🧪 Casos de Teste

### **Teste 1: Validação de Email**

**Request Inválido:**
```http
POST https://localhost:5001/api/auth/register
Content-Type: application/json

{
  "name": "João",
  "email": "email-invalido",
  "password": "Senha123"
}
```

**Response (400 Bad Request):**
```json
{
  "error": "Ocorreram um ou mais erros de validação.",
  "errors": {
    "Email": ["Email inválido"]
  },
  "statusCode": 400,
  "timestamp": "2024-01-15T10:00:00Z",
  "type": "Validation"
}
```

---

### **Teste 2: Senha Muito Curta**

**Request:**
```json
{
  "name": "João",
  "email": "joao@email.com",
  "password": "123"
}
```

**Response (400 Bad Request):**
```json
{
  "error": "Ocorreram um ou mais erros de validação.",
  "errors": {
    "Password": ["Senha deve ter no mínimo 6 caracteres"]
  },
  "statusCode": 400,
  "type": "Validation"
}
```

---

### **Teste 3: Email Já Cadastrado**

**Request:**
```json
{
  "name": "Maria",
  "email": "maria@email.com",
  "password": "Senha123"
}
```

**Response (400 Bad Request):**
```json
{
  "error": "Email já está cadastrado",
  "statusCode": 400,
  "type": "BusinessRule"
}
```

---

### **Teste 4: Credenciais Inválidas**

**Request:**
```json
{
  "email": "maria@email.com",
  "password": "senha-errada"
}
```

**Response (401 Unauthorized):**
```json
{
  "error": "Email ou senha inválidos",
  "statusCode": 401,
  "type": "Unauthorized"
}
```

---

### **Teste 5: Token Ausente**

**Request:**
```http
GET https://localhost:5001/api/account
```

**Response (401 Unauthorized):**
```json
{
  "error": "Acesso não autorizado.",
  "statusCode": 401,
  "type": "Unauthorized"
}
```

---

### **Teste 6: Token Inválido**

**Request:**
```http
GET https://localhost:5001/api/account
Authorization: Bearer token-invalido
```

**Response (401 Unauthorized):**
```json
{
  "error": "Acesso não autorizado.",
  "statusCode": 401,
  "type": "Unauthorized"
}
```

---

## 📱 Exemplos de Integração

### **JavaScript/Fetch API**

```javascript
// Registrar
async function register(name, email, password) {
  const response = await fetch('https://localhost:5001/api/auth/register', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name, email, password })
  });
  
  const data = await response.json();
  localStorage.setItem('token', data.token);
  return data;
}

// Login
async function login(email, password) {
  const response = await fetch('https://localhost:5001/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  
  const data = await response.json();
  localStorage.setItem('token', data.token);
  return data;
}

// Fazer requisição autenticada
async function getAccounts() {
  const token = localStorage.getItem('token');
  
  const response = await fetch('https://localhost:5001/api/account', {
    headers: { 
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    }
  });
  
  return response.json();
}
```

---

### **C# / HttpClient**

```csharp
public class FinancialApiClient
{
    private readonly HttpClient _client;
    private string _token;

    public FinancialApiClient()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
    }

    public async Task<AuthResponse> RegisterAsync(string name, string email, string password)
    {
        var request = new { name, email, password };
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        response.EnsureSuccessStatusCode();
        
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        _token = authResponse.Token;
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _token);
        
        return authResponse;
    }

    public async Task<List<Account>> GetAccountsAsync()
    {
        var response = await _client.GetAsync("/api/account");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Account>>();
    }
}
```

---

### **Python / Requests**

```python
import requests

class FinancialApiClient:
    def __init__(self):
        self.base_url = "https://localhost:5001"
        self.token = None
    
    def register(self, name, email, password):
        response = requests.post(
            f"{self.base_url}/api/auth/register",
            json={"name": name, "email": email, "password": password}
        )
        data = response.json()
        self.token = data["token"]
        return data
    
    def login(self, email, password):
        response = requests.post(
            f"{self.base_url}/api/auth/login",
            json={"email": email, "password": password}
        )
        data = response.json()
        self.token = data["token"]
        return data
    
    def get_accounts(self):
        headers = {"Authorization": f"Bearer {self.token}"}
        response = requests.get(
            f"{self.base_url}/api/account",
            headers=headers
        )
        return response.json()
```

---

## 🎯 Fluxo Completo de Uso da API

```mermaid
sequenceDiagram
    participant Client
    participant API
    participant Database
    
    Client->>API: POST /api/auth/register
    API->>Database: Criar usuário
    Database-->>API: Usuário criado
    API-->>Client: Token JWT
    
    Client->>API: POST /api/account (com token)
    API->>Database: Criar conta
    Database-->>API: Conta criada
    API-->>Client: Dados da conta
    
    Client->>API: POST /api/transaction (com token)
    API->>Database: Criar transação
    Database-->>API: Transação criada
    API-->>Client: Dados da transação
    
    Client->>API: GET /api/account/{id} (com token)
    API->>Database: Buscar conta
    Database-->>API: Dados da conta
    API-->>Client: Conta com saldo
```

---

## ✅ Checklist de Teste Manual

- [ ] Registrar novo usuário
- [ ] Fazer login com usuário existente
- [ ] Tentar login com senha errada (deve falhar)
- [ ] Tentar acessar endpoint sem token (deve retornar 401)
- [ ] Criar conta bancária
- [ ] Listar contas
- [ ] Criar categorias (Receita e Despesa)
- [ ] Adicionar transação de receita
- [ ] Adicionar transação de despesa
- [ ] Consultar saldo da conta
- [ ] Criar segunda conta
- [ ] Fazer transferência entre contas
- [ ] Verificar saldos após transferência
- [ ] Listar todas as transações
- [ ] Filtrar transações por período
- [ ] Aguardar token expirar (8h) e tentar usar (deve retornar 401)
- [ ] Fazer novo login após expiração

---

**✅ Exemplos prontos para uso!** 🚀

Use estes exemplos como base para testar e integrar a API no seu frontend ou aplicação.
