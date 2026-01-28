# Guia de Configuração - SQL Server

## Configuração do SQL Server

### Opção 1: SQL Server via Docker (Recomendado)

```bash
# Iniciar SQL Server em container
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=senha123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

# Aguardar SQL Server iniciar (cerca de 30 segundos)
# Verificar se está rodando
docker ps
```

### Opção 2: SQL Server Local

1. Instale o SQL Server 2022 ou superior
2. Configure a instância padrão
3. Anote a senha do usuário `sa` ou crie um usuário específico

## Configuração das Connection Strings

### PropostaService

Edite `src/PropostaService/PropostaService.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PropostaDB;User Id=sa;Password=senha123;TrustServerCertificate=True;"
  }
}
```

### ContratacaoService

Edite `src/ContratacaoService/ContratacaoService.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ContratacaoDB;User Id=sa;Password=senha123;TrustServerCertificate=True;"
  }
}
```

**Nota**: Ajuste `Server`, `User Id` e `Password` conforme sua instalação.

## Criar Bancos de Dados

### Opção 1: Via Script SQL

```bash
# Conectar ao SQL Server
sqlcmd -S localhost -U sa -P senha123

# Executar script
sqlcmd -S localhost -U sa -P senha123 -i scripts/init-databases.sql
```

### Opção 2: Via Entity Framework Migrations (Recomendado)

As migrations criarão os bancos automaticamente se não existirem.

## Executar Migrations

### PropostaService

```bash
cd src/PropostaService/PropostaService.API

# Criar migration
dotnet ef migrations add InitialCreate --project ../PropostaService.Infrastructure

# Aplicar migration (cria o banco se não existir)
dotnet ef database update --project ../PropostaService.Infrastructure
```

### ContratacaoService

```bash
cd src/ContratacaoService/ContratacaoService.API

# Criar migration
dotnet ef migrations add InitialCreate --project ../ContratacaoService.Infrastructure

# Aplicar migration (cria o banco se não existir)
dotnet ef database update --project ../ContratacaoService.Infrastructure
```

## Verificar Conexão

### Teste de Conexão

```bash
# Testar conexão com SQL Server
sqlcmd -S localhost -U sa -P senha123 -Q "SELECT @@VERSION"
```

### Verificar Bancos Criados

```bash
sqlcmd -S localhost -U sa -P senha123 -Q "SELECT name FROM sys.databases WHERE name IN ('PropostaDB', 'ContratacaoDB')"
```

## Troubleshooting

### Erro: "Cannot open database"

- Verifique se o SQL Server está rodando
- Confirme a connection string
- Verifique se o usuário tem permissões

### Erro: "Login failed"

- Verifique usuário e senha
- Para SQL Server em Docker, use `sa` e a senha definida

### Erro: "TrustServerCertificate"

- Adicione `TrustServerCertificate=True;` na connection string
- Ou configure certificado SSL no SQL Server

## Usando Docker Compose

O `docker-compose.yml` já está configurado para:
- Criar container SQL Server automaticamente
- Configurar connection strings nos serviços
- Aguardar SQL Server estar pronto antes de iniciar os serviços

```bash
docker-compose up -d
```

Os bancos serão criados automaticamente quando os serviços iniciarem e executarem as migrations.
