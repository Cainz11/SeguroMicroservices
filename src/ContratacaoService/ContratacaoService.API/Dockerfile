FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/ContratacaoService/ContratacaoService.API/ContratacaoService.API.csproj", "src/ContratacaoService/ContratacaoService.API/"]
COPY ["src/ContratacaoService/ContratacaoService.Application/ContratacaoService.Application.csproj", "src/ContratacaoService/ContratacaoService.Application/"]
COPY ["src/ContratacaoService/ContratacaoService.Domain/ContratacaoService.Domain.csproj", "src/ContratacaoService/ContratacaoService.Domain/"]
COPY ["src/ContratacaoService/ContratacaoService.Infrastructure/ContratacaoService.Infrastructure.csproj", "src/ContratacaoService/ContratacaoService.Infrastructure/"]

RUN dotnet restore "src/ContratacaoService/ContratacaoService.API/ContratacaoService.API.csproj"
COPY . .
WORKDIR "/src/src/ContratacaoService/ContratacaoService.API"
RUN dotnet build "ContratacaoService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContratacaoService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContratacaoService.API.dll"]
