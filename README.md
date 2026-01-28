# Seguro Microservices

Sistema de gerenciamento de propostas de seguro com arquitetura de microserviços.

## Estrutura

- **PropostaService**: Gerencia propostas de seguro (criar, listar, alterar status)
- **ContratacaoService**: Realiza contratações de propostas aprovadas

## Tecnologias

.NET 10
SQL Server
Docker 

## Como Rodar

### 1. Subir o banco de dados

```powershell
docker-compose up -d sqlserver
```

Ou manualmente:
```powershell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Senha123!" -e "MSSQL_PID=Developer" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Aplicar migrations

```powershell
# PropostaService
cd src\PropostaService\PropostaService.API
dotnet ef database update --project ..\PropostaService.Infrastructure

# ContratacaoService
cd ..\..\ContratacaoService\ContratacaoService.API
dotnet ef database update --project ..\ContratacaoService.Infrastructure
```

### 3. Executar os serviços

**Terminal 1:**
```powershell
cd src\PropostaService\PropostaService.API
dotnet run
```

**Terminal 2:**
```powershell
cd src\ContratacaoService\ContratacaoService.API
dotnet run
```

## Acessar as APIs

- PropostaService: http://localhost:5001/swagger
- ContratacaoService: http://localhost:5002/swagger

## Configuração do Banco

- **Servidor**: localhost:1433
- **Usuário**: sa
- **Senha**: Senha123!
- **Bancos**: PropostaDB, ContratacaoDB

## Endpoints Principais

### PropostaService

- `POST /api/propostas` - Criar proposta
- `GET /api/propostas` - Listar propostas
- `GET /api/propostas/{id}` - Obter proposta
- `PUT /api/propostas/{id}/status` - Alterar status (1=EmAnálise, 2=Aprovada, 3=Rejeitada)

### ContratacaoService

- `POST /api/contratacoes` - Contratar proposta
- `GET /api/contratacoes` - Listar contratações

## Testes

```powershell
dotnet test
```
