# Guia Rápido de Início

## Executar o Projeto Rapidamente

### Pré-requisitos
- .NET 10 SDK instalado
- Docker Desktop (para SQL Server)

### Passos

#### 1. Iniciar SQL Server
```powershell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=senha123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

#### 2. Criar bancos de dados e aplicar migrations

**PropostaService**:
```powershell
cd C:\Users\Caio\seguro-microservices\src\PropostaService\PropostaService.API
dotnet ef migrations add InitialCreate --project ..\PropostaService.Infrastructure
dotnet ef database update --project ..\PropostaService.Infrastructure
```

**ContratacaoService**:
```powershell
cd C:\Users\Caio\seguro-microservices\src\ContratacaoService\ContratacaoService.API
dotnet ef migrations add InitialCreate --project ..\ContratacaoService.Infrastructure
dotnet ef database update --project ..\ContratacaoService.Infrastructure
```

#### 3. Executar os serviços

**Terminal 1 - PropostaService**:
```powershell
cd C:\Users\Caio\seguro-microservices\src\PropostaService\PropostaService.API
dotnet run
```

**Terminal 2 - ContratacaoService**:
```powershell
cd C:\Users\Caio\seguro-microservices\src\ContratacaoService\ContratacaoService.API
dotnet run
```

#### 4. Testar as APIs

Acesse os Swagger UIs:
- **PropostaService**: http://localhost:5001/swagger
- **ContratacaoService**: http://localhost:5002/swagger

#### 5. Exemplo de Uso

**Criar uma proposta**:
```http
POST http://localhost:5001/api/propostas
Content-Type: application/json

{
  "seguradoNome": "João Silva",
  "seguradoCpf": "12345678909",
  "valorPremio": 1500.00
}
```

**Aprovar a proposta**:
```http
PUT http://localhost:5001/api/propostas/{id}/status
Content-Type: application/json

{
  "status": 2
}
```
Status: 1 = EmAnalise, 2 = Aprovada, 3 = Rejeitada

**Contratar a proposta**:
```http
POST http://localhost:5002/api/contratacoes
Content-Type: application/json

{
  "propostaId": "{id-da-proposta}"
}
```

## Executar Testes

```powershell
cd C:\Users\Caio\seguro-microservices\tests\PropostaService.Tests
dotnet test
```

## Health Checks

Verificar saúde dos serviços:
- PropostaService: http://localhost:5001/health
- ContratacaoService: http://localhost:5002/health

## Parar os Serviços

1. Parar as aplicações: `Ctrl+C` nos terminais
2. Parar SQL Server: `docker stop sqlserver`
3. Remover container: `docker rm sqlserver`
