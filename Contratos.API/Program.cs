using System.Text;
using AutoMapper;
using Contratos.Application.Interfaces;
using Contratos.Application.Mapping;
using Contratos.Application.Services;
using Contratos.Domain.Repositories;
using Contratos.Infrastructure.Auth;
using Contratos.Infrastructure.Persistence;
using Contratos.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// =========================================
// 1) CONFIGURAÇÕES / UTILS
// =========================================
string GetRequired(string key)
{
    var v = builder.Configuration[key];
    if (string.IsNullOrWhiteSpace(v))
        throw new InvalidOperationException($"Config ausente: '{key}'. Defina em appsettings.json ou App Service → Configuration.");
    return v;
}

// =========================================
// 2) DATABASE (SQLite)
//    ConnectionStrings:Default  (ex.: Data Source=D:\home\data\contratos.db)
// =========================================
var connString = builder.Configuration.GetConnectionString("Default")
                 ?? throw new InvalidOperationException("ConnectionStrings:Default não configurada.");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(connString));

// Garante que o diretório do arquivo .db exista (útil no Azure D:\home\data)
try
{
    var dataSource = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(connString).DataSource;
    if (!string.IsNullOrWhiteSpace(dataSource))
    {
        var fullPath = dataSource;
        // Se vier relativo, não alteramos; se vier absoluto, tentamos criar a pasta
        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);
    }
}
catch
{
    // Ignora: se não for um caminho de arquivo (ex.: outras variações), segue normalmente
}

// =========================================
// 3) REPOSITÓRIOS / UoW / SERVIÇOS
// =========================================
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();

// =========================================
// 4) AutoMapper
// =========================================
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// =========================================
// 5) AUTENTICAÇÃO JWT
//    Requer App Settings: Jwt:Key, Jwt:Issuer, Jwt:Audience
// =========================================
var jwtKey = GetRequired("Jwt:Key");
var jwtIssuer = GetRequired("Jwt:Issuer");
var jwtAudience = GetRequired("Jwt:Audience");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = key
        };
    });

// =========================================
builder.Services.AddControllers();

// CORS liberado para facilitar testes (ajuste para WithOrigins(...) em produção)
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// =========================================
// 6) SWAGGER (com JWT)
// =========================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contratos API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Insira: Bearer {seu_token_jwt}"
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// =========================================
// 7) BUILD APP
// =========================================
var app = builder.Build();

// =========================================
// 8) MIGRATIONS na inicialização (com log)
//    Se falhar, loga o erro mas não derruba o host
// =========================================
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider
                      .GetRequiredService<ILoggerFactory>()
                      .CreateLogger("Startup");

    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        logger.LogInformation("Migrations aplicadas com sucesso.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao aplicar migrations. Verifique ConnectionStrings e permissões.");
        // Se preferir falhar o start, troque para: throw;
    }
}

// =========================================
// 9) PIPELINE
// =========================================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contratos API v1");
    // c.RoutePrefix = string.Empty; // opcional: expõe Swagger em "/"
});

// Em App Service (Windows/IIS), o HTTPS é feito na frente pelo IIS.
// Se quiser HSTS só em produção:
// if (app.Environment.IsProduction()) app.UseHsts();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// MUITO IMPORTANTE: não force porta no App Service Windows.
// Deixe o IIS/ANCM gerenciar a porta.
app.Run();
