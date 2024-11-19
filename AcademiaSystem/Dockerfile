FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar o arquivo .csproj diretamente
COPY ["AcademiaSystem.csproj", "./"]

# Restaurar dependências
RUN dotnet restore "AcademiaSystem.csproj"

# Copiar o restante dos arquivos do projeto
COPY . .

# Compilar o projeto
RUN dotnet build "AcademiaSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AcademiaSystem.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT Development
ENTRYPOINT ["dotnet", "AcademiaSystem.dll"]
