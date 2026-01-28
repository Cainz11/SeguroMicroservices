FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/PropostaService/PropostaService.API/PropostaService.API.csproj", "src/PropostaService/PropostaService.API/"]
COPY ["src/PropostaService/PropostaService.Application/PropostaService.Application.csproj", "src/PropostaService/PropostaService.Application/"]
COPY ["src/PropostaService/PropostaService.Domain/PropostaService.Domain.csproj", "src/PropostaService/PropostaService.Domain/"]
COPY ["src/PropostaService/PropostaService.Infrastructure/PropostaService.Infrastructure.csproj", "src/PropostaService/PropostaService.Infrastructure/"]

RUN dotnet restore "src/PropostaService/PropostaService.API/PropostaService.API.csproj"
COPY . .
WORKDIR "/src/src/PropostaService/PropostaService.API"
RUN dotnet build "PropostaService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PropostaService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PropostaService.API.dll"]
