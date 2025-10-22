# 💼 **Portfolio API — Sistema de Gerenciamento de Investimentos (.NET 8)**

[![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Build](https://img.shields.io/badge/build-passing-brightgreen?style=flat&logo=githubactions)](https://github.com/rmyotin/Portfolio-Financeiro)
[![Tests](https://img.shields.io/badge/tests-100%25-success?style=flat&logo=xunit)](https://github.com/rmyotin/Portfolio-Financeiro/actions)
[![License](https://img.shields.io/badge/license-MIT-lightgrey?style=flat)](LICENSE)

Desenvolvido por **Rodrigo Myotin**  
📧 [myotin@yahoo.com.br](mailto:myotin@yahoo.com.br)  
💼 [https://github.com/rmyotin/Portfolio-Financeiro](https://github.com/rmyotin/Portfolio-Financeiro)

---

## 🚀 **Sobre o Projeto**

A **Portfolio API** é uma aplicação desenvolvida em **.NET 8 WebAPI** que gerencia **portfólios de investimento**, realizando cálculos de retorno, rebalanceamento automático e análise de risco.

💾 O projeto usa **banco InMemory** com dados do arquivo `SeedData.json`, permitindo execução imediata.

📊 Inclui métricas como:
- Retorno total e anualizado
- Volatilidade
- Sharpe Ratio
- Diversificação por setor
- Correlação entre ativos (coeficiente de Pearson)

---

## 🧱 **Arquitetura e Estrutura do Projeto**

```
Portfolio-Financeiro/
├── Controllers/          # Endpoints da API
├── Services/             # Lógica de negócio e cálculos financeiros
├── Repositories/         # Acesso a dados (InMemory)
├── Models/               # Entidades e DTOs
├── Infrastructure/       # Contexto, SeedData, EF Core
└── Tests/                # Testes unitários (xUnit)
```

🧩 **Padrões aplicados:**
- Clean Architecture  
- SOLID  
- Repository Pattern  
- Dependency Injection  
- Documentação Swagger  
- Testes com Moq + FluentAssertions  

---

## 💡 **Principais Entidades**

### 💰 `Asset`
Representa um ativo financeiro (ação, fundo, etc.).

| Campo | Tipo | Descrição |
|--------|------|------------|
| Symbol | string | Código do ativo (ex: PETR4) |
| Name | string | Nome completo |
| Sector | string | Setor econômico |
| CurrentPrice | double | Preço atual |
| PriceHistory | List<PriceHistory> | Histórico de preços diários (ISO 8601) |

---

### 💼 `Portfolio`
Agrupa as posições do investidor.

| Campo | Tipo | Descrição |
|--------|------|------------|
| Name | string | Nome do portfólio |
| UserId | string | Identificador do investidor |
| TotalInvestment | double | Valor total investido |
| Positions | List<Position> | Lista de ativos e quantidades |

---

## ⚙️ **Endpoints Principais**

### 🔹 AssetsController
| Método | Endpoint | Descrição |
|---------|-----------|------------|
| GET | `/api/assets` | Lista todos os ativos |
| GET | `/api/assets/{id}` | Retorna ativo por ID |
| GET | `/api/assets/search?symbol=PETR4` | Busca por símbolo |
| POST | `/api/assets` | Cria ativo |
| PUT | `/api/assets/{id}/price` | Atualiza preço |
| DELETE | `/api/assets/{id}` | Exclui ativo |

---

### 🔹 PortfoliosController
| Método | Endpoint | Descrição |
|---------|-----------|------------|
| GET | `/api/portfolios` | Lista todos os portfólios |
| GET | `/api/portfolios/{id}` | Detalha um portfólio |
| POST | `/api/portfolios` | Cria portfólio |
| POST | `/api/portfolios/{id}/positions` | Adiciona posição |
| PUT | `/api/portfolios/{id}/positions/{positionId}` | Atualiza posição |
| DELETE | `/api/portfolios/{id}/positions/{positionId}` | Remove posição |

---

### 🔹 AnalyticsController
| Método | Endpoint | Descrição |
|---------|-----------|------------|
| GET | `/api/portfolios/{id}/analytics/performance` | Retorno total e anualizado |
| GET | `/api/portfolios/{id}/analytics/risk-analysis` | Análise de risco (Sharpe e volatilidade) |
| GET | `/api/portfolios/{id}/analytics/rebalancing` | Sugestão de rebalanceamento |
| GET | `/api/portfolios/{id}/analytics/diversification` | Diversificação setorial |
| GET | `/api/portfolios/{id}/analytics/correlations` | Correlação entre ativos |

---

## 🧮 **Fórmulas Financeiras Implementadas**

| Métrica | Fórmula | Descrição |
|----------|----------|------------|
| **Retorno Total (%)** | `(ValorAtual - Investimento) / Investimento * 100` | Crescimento acumulado |
| **Retorno Anualizado (%)** | `(1 + RetTotal/100)^(365 / dias) - 1` | Retorno equivalente anual |
| **Volatilidade** | `√Σ(x - média)² / média` | Risco (variação dos preços) |
| **Sharpe Ratio** | `(Retorno - Selic) / Volatilidade` | Retorno ajustado ao risco |
| **Correlação (ρ)** | `Σ((A - Ā)(B - B̄)) / √(Σ(A - Ā)² * Σ(B - B̄)²)` | Dependência estatística entre ativos |

---

## 📊 **Exemplo de Resultado – Correlação entre Ativos**

**GET** `/api/portfolios/1/analytics/correlations`

```json
{
  "portfolio": "Portfólio Crescimento",
  "correlations": [
    { "assetA": "PETR4", "assetB": "VALE3", "correlationCoefficient": 0.96 },
    { "assetA": "PETR4", "assetB": "ITUB4", "correlationCoefficient": 0.22 },
    { "assetA": "VALE3", "assetB": "ITUB4", "correlationCoefficient": 0.18 }
  ]
}
```

📘 Interpretação:
- **+1.0** → Ativos altamente correlacionados  
- **0.0** → Sem correlação  
- **–1.0** → Movimentos opostos  

---

## 🔁 **Rebalanceamento de Portfólio**

**GET** `/api/portfolios/{id}/analytics/rebalancing`

```json
[
  { "asset": "PETR4", "action": "SELL", "amount": 1500.00 },
  { "asset": "ITUB4", "action": "BUY", "amount": 1200.00 }
]
```

📈 Regras:
- Custos de transação: `0.3%`
- Ignora valores < R$100
- Ignora variações < 1%
- Minimiza número de operações

---

## 🧪 **Testes Unitários**

Executar todos os testes:

```bash
dotnet test
```

📊 Cobertura:
- ✅ Cálculo de retorno total e anualizado  
- ✅ Sugestões de rebalanceamento  
- ✅ Cálculo de Sharpe Ratio e concentração  

---

## ⚙️ **Como Executar o Projeto**

### 🧰 Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022 ou VS Code

### ▶️ Rodando a API

```bash
dotnet run --project Portifolio.Controllers
```

Acesse o Swagger:
```
https://localhost:5001/swagger
```

---

## 🗃️ **Banco de Dados**

- **InMemoryDatabase**
- Dados carregados via `SeedData.json`
- Inclui ativos, portfólios e histórico de preços

---

## 📅 **Formato de Datas**

Todas as datas seguem o padrão **ISO 8601 (`yyyy-MM-dd`)**

```json
"createdAt": "2024-10-21"
```

---

## ✅ **Status Final**

| Requisito | Status |
|------------|----------|
| CRUD de ativos e portfólios | ✅ |
| Cálculos de performance | ✅ |
| Sistema de rebalanceamento | ✅ |
| Análise de risco e diversificação | ✅ |
| Correlação entre ativos | ✅ |
| Testes unitários | ✅ |
| Documentação Swagger | ✅ |
| README técnico | ✅ |

---

## 🧠 **Autor**

**Rodrigo Myotin**  
Desenvolvedor .NET
📧 [myotin@yahoo.com.br](mailto:myotin@yahoo.com.br)  
💼 [https://github.com/rmyotin/Portfolio-Financeiro](https://github.com/rmyotin/Portfolio-Financeiro)

---
