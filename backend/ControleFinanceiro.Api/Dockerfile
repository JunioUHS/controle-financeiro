FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia a solução e o projeto
COPY ControleFinanceiro.sln ./
COPY ControleFinanceiro.Api/ControleFinanceiro.Api.csproj ./ControleFinanceiro.Api/

# Restaura as dependências
RUN dotnet restore ./ControleFinanceiro.Api/ControleFinanceiro.Api.csproj

# Copia o restante dos arquivos do projeto
COPY ControleFinanceiro.Api/ ./ControleFinanceiro.Api/

# Publica a aplicação
RUN dotnet publish ./ControleFinanceiro.Api/ControleFinanceiro.Api.csproj -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "ControleFinanceiro.Api.dll"]

EXPOSE 5097