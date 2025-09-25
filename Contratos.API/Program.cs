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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------- DB (SQLite) ----------
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// ---------- Repositórios / UoW ----------
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ---------- Serviços de aplicação ----------
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();

// ---------- AutoMapper ----------
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// ---------- Auth JWT ----------
var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key
        };
    });

// ---------- Controllers / CORS ----------
builder.Services.AddControllers();

// Para dev/Swagger local deixamos liberado.
// Em produção, restrinja com .WithOrigins("https://seu-front.com")
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// ---------- Swagger (com JWT) ----------
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
        Description = "Insira: Bearer {seu_token_jwt}",
    };
    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// ---------- Migrations (em vez de EnsureCreated) ----------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ---------- Pipeline ----------
// Swagger SEM depender do ambiente para facilitar testes
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contratos API v1"); // caminho relativo evita mixed content
    // c.RoutePrefix = string.Empty; // descomente se quiser Swagger na raiz "/"
});

// IMPORTANTE: para evitar o erro do Swagger em dev, NÃO redirecionamos para HTTPS aqui.
// Quando for publicar, você pode reativar:
// if (app.Environment.IsProduction()) { app.UseHsts(); app.UseHttpsRedirection(); }

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ---------- Porta (útil em PaaS) ----------
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");
