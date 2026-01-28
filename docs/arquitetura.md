# Arquitetura do Sistema

## Visão Geral

O sistema utiliza **Arquitetura Hexagonal (Ports & Adapters)** combinada com **Microserviços** e **DDD (Domain-Driven Design)**.

## Arquitetura Hexagonal

A arquitetura hexagonal separa a lógica de negócio (core) das tecnologias externas (adapters), através de interfaces (ports).

### Camadas

#### 1. Domain (Núcleo)
- **Entidades**: `Proposta`, `Contratacao`
- **Repositórios (Ports)**: Interfaces que definem contratos
- **Regras de Negócio**: Lógica pura, sem dependências externas

#### 2. Application (Casos de Uso)
- **Interfaces (Ports)**: Contratos para serviços de aplicação
- **Services**: Implementação dos casos de uso
- **Orquestração**: Coordenação entre domínio e infraestrutura

#### 3. Infrastructure (Adapters)
- **Repositórios**: Implementação concreta usando Entity Framework
- **DbContext**: Configuração do SQL Server
- **HTTP Clients**: Comunicação entre microserviços
- **Implementações**: Adaptadores para tecnologias externas

#### 4. API (Interface Externa)
- **Controllers**: Endpoints REST
- **DTOs**: Objetos de transferência de dados
- **Configuração**: DI, Middleware, Swagger

## Microserviços

### PropostaService
```
┌─────────────────────────────────────┐
│         API (Controllers)            │
├─────────────────────────────────────┤
│      Application (Use Cases)         │
├─────────────────────────────────────┤
│         Domain (Entities)            │
├─────────────────────────────────────┤
│   Infrastructure (Repositories)    │
│         SQL Server (DB)             │
└─────────────────────────────────────┘
```

### ContratacaoService
```
┌─────────────────────────────────────┐
│         API (Controllers)            │
├─────────────────────────────────────┤
│      Application (Use Cases)         │
├─────────────────────────────────────┤
│         Domain (Entities)            │
├─────────────────────────────────────┤
│   Infrastructure (Repositories)     │
│   HTTP Client (PropostaService)     │
│         SQL Server (DB)             │
└─────────────────────────────────────┘
```

## Comunicação entre Microserviços

```
ContratacaoService ──HTTP REST──> PropostaService
     │                                  │
     │                                  │
     └──────────SQL Server──────────────┘
```

- **HTTP REST**: ContratacaoService consulta status da proposta
- **SQL Server**: Cada microserviço tem seu próprio banco de dados

## Fluxo de Dados

### Criar Proposta
1. Cliente → API (PropostaService)
2. Controller → Application Service
3. Application Service → Domain Entity
4. Application Service → Repository (Port)
5. Repository (Adapter) → SQL Server

### Contratar Proposta
1. Cliente → API (ContratacaoService)
2. Controller → Application Service
3. Application Service → HTTP Client (Port)
4. HTTP Client (Adapter) → PropostaService API
5. Application Service → Repository (Port)
6. Repository (Adapter) → SQL Server

## Princípios Aplicados

### SOLID
- **S**ingle Responsibility: Cada classe tem uma responsabilidade
- **O**pen/Closed: Extensível via interfaces (ports)
- **L**iskov Substitution: Implementações substituíveis
- **I**nterface Segregation: Interfaces específicas
- **D**ependency Inversion: Dependências de abstrações

### Clean Code
- Nomes descritivos
- Funções pequenas e focadas
- Separação de responsabilidades
- Testabilidade

### DDD
- Entidades ricas com comportamento
- Agregados bem definidos
- Repositórios como abstrações
- Linguagem ubíqua

## Banco de Dados

### PropostaDB (SQL Server)
- Tabela: `Propostas`
- Campos: Id, SeguradoNome, SeguradoCpf, ValorPremio, Status, DataCriacao, DataAtualizacao

### ContratacaoDB (SQL Server)
- Tabela: `Contratacoes`
- Campos: Id, PropostaId, DataContratacao
- Índice único em PropostaId

## Benefícios da Arquitetura

1. **Testabilidade**: Fácil mockar ports
2. **Manutenibilidade**: Mudanças isoladas
3. **Flexibilidade**: Trocar implementações facilmente
4. **Escalabilidade**: Microserviços independentes
5. **Desacoplamento**: Core independente de tecnologias
