# Como Executar no Visual Studio

## Passo 1: Abrir a Solução

1. Abra o **Visual Studio 2022** (ou versão mais recente)
2. Clique em **File > Open > Project/Solution**
3. Navegue até a pasta do projeto e selecione o arquivo `SeguroMicroservices.sln`
4. Clique em **Open**

## Passo 2: Configurar Múltiplos Projetos de Inicialização

Para executar ambos os serviços simultaneamente:

1. Clique com o botão direito na **solução** no **Solution Explorer**
2. Selecione **Properties** (ou **Configure Startup Projects...**)
3. Na janela que abrir, selecione **Multiple startup projects**
4. Configure os projetos:
   - **PropostaService.API**: Selecione **Start**
   - **ContratacaoService.API**: Selecione **Start**
   - Todos os outros projetos: Deixe como **None**
5. Clique em **OK**

## Passo 3: Executar os Serviços

### Opção A: Executar Ambos Simultaneamente (Recomendado)

1. Pressione **F5** ou clique no botão **Start** (▶️)
2. Ambos os serviços serão iniciados automaticamente
3. O Visual Studio abrirá uma janela de console para cada serviço

### Opção B: Executar Individualmente

1. Clique com o botão direito no projeto que deseja executar (ex: `PropostaService.API`)
2. Selecione **Set as Startup Project**
3. Pressione **F5** ou clique em **Start**

## Passo 4: Acessar as APIs

Após iniciar os serviços, acesse:

- **PropostaService Swagger**: http://localhost:5001/swagger
- **ContratacaoService Swagger**: http://localhost:5002/swagger

## Configuração de Perfis de Execução

Cada projeto já está configurado com perfis no `launchSettings.json`:

### PropostaService.API
- **Perfil HTTP**: `http://localhost:5001`
- **Perfil HTTPS**: `https://localhost:7280` e `http://localhost:5001`

### ContratacaoService.API
- **Perfil HTTP**: `http://localhost:5002`
- **Perfil HTTPS**: `https://localhost:7129` e `http://localhost:5002`

Para alterar o perfil:
1. Clique na seta ao lado do botão **Start**
2. Selecione o perfil desejado (HTTP ou HTTPS)

## Pré-requisitos

Antes de executar, certifique-se de que:

1. **SQL Server está rodando**:
   ```powershell
   docker-compose up -d sqlserver
   ```
   Ou verifique se o SQL Server local está em execução

2. **Bancos de dados foram criados**:
   - As migrations devem ter sido aplicadas
   - Os bancos `PropostaDB` e `ContratacaoDB` devem existir

## Debugging

Para fazer debug:

1. Coloque **breakpoints** no código onde deseja parar
2. Pressione **F5** para iniciar em modo debug
3. O Visual Studio pausará nos breakpoints quando o código for executado

## Parar os Serviços

- Pressione **Shift + F5** ou clique no botão **Stop** (■)
- Ou feche as janelas de console dos serviços

## Troubleshooting

### Erro: "Port already in use"
- Verifique se outro processo está usando as portas 5001 ou 5002
- Altere as portas no `launchSettings.json` se necessário

### Erro: "Cannot connect to database"
- Verifique se o SQL Server está rodando
- Confirme as connection strings nos arquivos `appsettings.json`

### Serviços não iniciam
- Verifique os logs na janela de **Output** do Visual Studio
- Verifique se todas as dependências NuGet foram restauradas (Build > Restore NuGet Packages)
