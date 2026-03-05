# 📊 API-FINANCEIRO

> 🚧 **Projeto em andamento**  
> Esta API está em desenvolvimento ativo. Funcionalidades podem ser adicionadas, modificadas ou removidas conforme a evolução do projeto.

---

## 📌 Sobre o Projeto

A **API-FINANCEIRO** é uma API RESTful desenvolvida em **C# com .NET**, com o objetivo de fornecer um backend estruturado para sistemas de gestão financeira.

O projeto está sendo construído com foco em:

- Organização em camadas
- Boas práticas de arquitetura
- Separação de responsabilidades
- Escalabilidade futura
- Aplicação de conceitos de Clean Code

---

## 🎯 Objetivo

Criar uma API completa para controle financeiro que permita:

- Cadastro de receitas
- Cadastro de despesas
- Controle de categorias
- Cálculo de saldo
- Organização de movimentações financeiras
- Futuramente: autenticação e controle por usuário

---

## 🛠️ Stack Tecnológica

- **Linguagem:** C#
- **Framework:** .NET
- **Arquitetura:** API REST
- **Banco de Dados:** (em definição / configurável)
- **ORM:** (caso esteja utilizando Entity Framework)

---

## 📁 Estrutura Atual do Projeto

```plaintext
API-FINANCEIRO/
├── API/                   # Projeto principal da API
├── Controllers/           # Endpoints
├── Models/                # Entidades de domínio
├── Services/              # Regras de negócio
├── Data/                  # Contexto do banco
├── Migrations/            # Versionamento do banco (se aplicável)
└── README.md
