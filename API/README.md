# 💰 Financial Manager API

API REST para gerenciamento financeiro com suporte a transações e transferências entre contas.

## 🏗️ Arquitetura

Projeto desenvolvido seguindo **Clean Architecture** e **DDD (Domain-Driven Design)**:

```
├── Domain/                    # Entidades e Serviços de Domínio
├── Application/              # Casos de Uso e DTOs
├── Infrastructure/           # Persistência e Repositórios
└── API/                      # Controllers e Configuração
```

## 🚀 Tecnologias

- **.NET 9**
- **C# 13.0**
- **Entity Framework Core**
- **SQL Server**
- **Swagger/OpenAPI**
- **Health Checks**

## ⚙️ Configuração

### 1. Connection String

Edite o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FinancialManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 2. Migrations

```bash
dotnet ef database update --project FinancialManager.Infrastructure --startup-project FinancialManager.API
```

## 🏃 Executar

```bash
dotnet run --project FinancialManager.API
```

A API estará disponível em:
- **Swagger UI**: `https://localhost:5001`
- **Health Check**: `https://localhost:5001/health`

## 📋 Endpoints

### Transações

**POST** `/api/transaction`
```json
{
  "accountId": 1,
  "description": "Compra no mercado",
  "amount": 150.50,
  "date": "2024-01-15",
  "type": 1,
  "categoryId": 2
}
```

### Transferências

**POST** `/api/transfer`
```json
{
  "sourceAccountId": 1,
  "destinationAccountId": 2,
  "amount": 500.00,
  "description": "Transferência entre contas",
  "categoryId": 5
}
```

## 🔒 Segurança

### Produção

Para ambientes de produção, configure:

1. **HTTPS** obrigatório
2. **Autenticação JWT** (recomendado)
3. **CORS** restrito
4. **Connection String** via variáveis de ambiente ou Azure Key Vault

Exemplo com variável de ambiente:
```bash
export ConnectionStrings__DefaultConnection="Server=..."
```

## 🏥 Health Checks

O endpoint `/health` retorna:

```json
{
  "status": "Healthy",
  "checks": {
    "database": "Healthy"
  }
}
```

## 🛠️ Melhorias Implementadas

✅ **Middleware de Exceções Global** - Tratamento centralizado de erros  
✅ **Health Checks** - Monitoramento da API e banco de dados  
✅ **CORS Configurado** - Suporte para frontends  
✅ **Logging Estruturado** - Rastreabilidade de operações  
✅ **Swagger Aprimorado** - Documentação completa da API  
✅ **Validação de Connection String** - Segurança na inicialização  

## 📝 Licença

Projeto acadêmico desenvolvido para fins educacionais.

## 👥 Contribuidores

- [VictoRGBC](https://github.com/VictoRGBC)
