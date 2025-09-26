using System.Data.Common;
using Contratos.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Contratos.Teste
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        private DbConnection? _connection;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove o AppDbContext “real”
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor is not null) services.Remove(descriptor);

                // Cria conexão SQLite in-memory
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                services.AddDbContext<AppDbContext>(opts =>
                {
                    opts.UseSqlite(_connection);
                });

                // Troca o esquema de auth por um handler fake
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = FakeAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = FakeAuthHandler.SchemeName;
                }).AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>(
                    FakeAuthHandler.SchemeName, _ => { });

                // Constrói provider para rodar migrations/seed
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated(); // ou db.Database.Migrate();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connection?.Dispose();
        }
    }
}
