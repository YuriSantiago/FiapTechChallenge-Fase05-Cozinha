FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todas as pastas necessárias
COPY Core ./Core
COPY Infrastructure ./Infrastructure
COPY CozinhaConsumidor ./CozinhaConsumidor

WORKDIR /src/CozinhaConsumidor

RUN dotnet restore CozinhaConsumidor.csproj
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CozinhaConsumidor.dll"]