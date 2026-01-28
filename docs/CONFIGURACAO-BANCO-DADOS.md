# Configuração do Banco de Dados SQL Server

## Informações de Acesso

### Credenciais do SQL Server

- **Servidor**: `localhost` (ou `sqlserver` quando dentro da rede Docker)
- **Porta**: `1433`
- **Usuário**: `sa`
- **Senha**: `Senha123!`
- **Tipo**: SQL Server 2022 (Developer Edition)

### Bancos de Dados

1. **PropostaDB**
   - Usado pelo: `PropostaService`
   - Connection String: `Server=localhost;Database=PropostaDB;User Id=sa;Password=Senha123!;TrustServerCertificate=True;`

2. **ContratacaoDB**
   - Usado pelo: `ContratacaoService`
   - Connection String: `Server=localhost;Database=ContratacaoDB;User Id=sa;Password=Senha123!;TrustServerCertificate=True;`

## Configuração via Docker Compose

O SQL Server é configurado automaticamente no `docker-compose.yml`:

```yaml
sqlserver:
  image: mcr.microsoft.com/mssql/server:2022-latest
  container_name: seguro-sqlserver
  environment:
    - ACCEPT_EULA=Y
    - SA_PASSWORD=Senha123!
    - MSSQL_PID=Developer
  ports:
    - "1433:1433"
  volumes:
    - sqlserver_data:/var/opt/mssql
```

## Iniciar o SQL Server

### Opção 1: Via Docker Compose (Recomendado)
```powershell
docker-compose up -d sqlserver
```

### Opção 2: Via Docker Run
```powershell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Senha123!" -e "MSSQL_PID=Developer" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

## Verificar Status

```powershell
# Verificar se o container está rodando
docker ps --filter "name=sqlserver"

# Verificar logs
docker logs sqlserver

# Testar conexão
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Senha123!" -Q "SELECT @@VERSION"
```

## Conectar via SQL Server Management Studio (SSMS)

- **Server name**: `localhost,1433`
- **Authentication**: SQL Server Authentication
- **Login**: `sa`
- **Password**: `Senha123!`

## Nota de Segurança

⚠️ **IMPORTANTE**: A senha `Senha123!` é apenas para desenvolvimento. Em produção, use uma senha forte e segura!

Para alterar a senha, edite o arquivo `docker-compose.yml` e altere o valor de `SA_PASSWORD` em ambos os lugares (environment do sqlserver e nas connection strings dos serviços).
