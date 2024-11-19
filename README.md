### Atenção!!!

Antes de executar os comandos docker, dê o comando `cd AcademiaSystem`, pois o dockerfile está lá dentro

```markdown
# AcademiaSystem

Este é um sistema web desenvolvido com ASP.NET Core e Entity Framework, utilizando SQLite como banco de dados. A aplicação é destinada à gestão de clientes de uma academia, permitindo a criação, edição, visualização e exclusão de registros.

## Requisitos

Antes de rodar a aplicação, é necessário garantir que você tenha os seguintes pré-requisitos instalados:

- **Docker**: para rodar a aplicação e o banco de dados de forma isolada.
- **.NET 8.0 SDK**: para compilar e rodar a aplicação localmente, caso não utilize o Docker.

## Configuração do Docker

Para rodar a aplicação em containers Docker, siga os seguintes passos:

1. **Criar e rodar os containers Docker**:

   Para construir e rodar os containers, execute os seguintes comandos no terminal:

   
   docker build -t academiasystem .
   docker run -d -p 8080:8080 --name academiasystem academiasystem
   ```

   O primeiro comando irá compilar a imagem da aplicação. O segundo comando irá subir os containers da aplicação e do banco de dados SQLite.

   A aplicação estará disponível em `http://localhost:8080`.

## Estrutura do Projeto

O projeto está dividido nas seguintes camadas principais:

- **Program.cs**: Arquivo de configuração inicial da aplicação, onde o `DbContext` é configurado para o banco de dados SQLite. O arquivo também aplica automaticamente as migrações ao iniciar a aplicação.

- **ClienteController.cs**: Controlador que gerencia as operações CRUD para o modelo `ClienteModel`. Ele permite visualizar, adicionar, editar e excluir registros de clientes.

- **ClienteModel.cs**: Modelo de dados que representa um cliente. Contém as propriedades `Id`, `Nome`, `Endereco`, e `Email`.

- **Views**: Arquivos Razor que definem as páginas da aplicação (Index, Create, Edit, Delete, etc.) para a visualização e interação do usuário com os dados.

- **appsettings.json**: Arquivo de configuração onde é definida a string de conexão para o banco de dados SQLite.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "AcademiaSystemContext": "Data Source=AcademiaSystemContext.db"
  }
}
```

## Banco de Dados SQLite

A aplicação utiliza o SQLite como banco de dados. O arquivo de banco de dados será gerado automaticamente ao rodar a aplicação, caso não exista. O Entity Framework é responsável pela criação das tabelas de forma automática.

### Migrations

As migrações são aplicadas automaticamente ao iniciar a aplicação, através do seguinte trecho no `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AcademiaSystemContext>();
    dbContext.Database.Migrate();
}
```

Se você preferir executar as migrações manualmente, pode usar o comando abaixo dentro do diretório do projeto:

```bash
dotnet ef database update
```

## Dockerfile

O Dockerfile é usado para construir a imagem do Docker da aplicação. Ele utiliza a imagem base `mcr.microsoft.com/dotnet/aspnet:8.0` para a produção e `mcr.microsoft.com/dotnet/sdk:8.0` para a compilação.

```dockerfile
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
```

## Funcionalidades

- **Index**: Exibe a lista de todos os clientes cadastrados.
- **Create**: Permite criar um novo cliente.
- **Edit**: Permite editar os dados de um cliente existente.
- **Details**: Exibe os detalhes de um cliente específico.
- **Delete**: Permite excluir um cliente.

## Como Rodar Localmente

Se você preferir rodar a aplicação localmente sem o Docker, siga os passos abaixo:

1. Clone o repositório:
   ```bash
   git clone <URL_DO_REPOSITORIO>
   cd academia-system
   ```

2. Restaure as dependências do projeto:
   ```bash
   dotnet restore
   ```

3. Execute o projeto:
   ```bash
   dotnet run
   ```

A aplicação estará disponível em `https://localhost:5001` ou `http://localhost:5000`.

## Contribuindo

Se você quiser contribuir com o projeto, basta seguir os seguintes passos:

1. Faça um fork deste repositório.
2. Crie uma branch para sua feature ou correção (`git checkout -b feature/nome-da-feature`).
3. Faça as alterações necessárias e commit (`git commit -am 'Adiciona nova funcionalidade'`).
4. Envie sua branch para o repositório remoto (`git push origin feature/nome-da-feature`).
5. Abra um Pull Request.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
```
