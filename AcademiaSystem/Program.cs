using Microsoft.EntityFrameworkCore;
using AcademiaSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext para SQLite
builder.Services.AddDbContext<AcademiaSystemContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AcademiaSystemContext") ?? 
                      throw new InvalidOperationException("Connection string 'AcademiaSystemContext' not found.")));

// Adiciona os serviços ao contêiner
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Aplica as migrações automaticamente ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AcademiaSystemContext>();
    dbContext.Database.Migrate();  // Aplica as migrações pendentes, criando o banco de dados se necessário
}

// Configura o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // O valor padrão do HSTS é 30 dias. Pode ser alterado para produção conforme necessário
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Define as rotas padrão para o controlador
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cliente}/{action=Index}/{id?}");

app.Run();