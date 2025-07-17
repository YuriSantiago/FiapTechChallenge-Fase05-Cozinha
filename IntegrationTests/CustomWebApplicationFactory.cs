using Core.Entities;
using Core.Helpers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.RemoveAll<IConfigureOptions<AuthenticationOptions>>();
                services.RemoveAll<IConfigureOptions<Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerOptions>>();

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                services.AddAuthentication("TestScheme")
                  .AddScheme<AuthenticationSchemeOptions, JwtAuthHandlerSimulation>("TestScheme", options =>
                  {
                      options.TimeProvider = TimeProvider.System;
                  });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                SeedDatabase(db).Wait();
            });

        }

        private static async Task SeedDatabase(ApplicationDbContext context)
        {
            context.Pedidos.RemoveRange(context.Pedidos);
            context.PedidosControleCozinha.RemoveRange(context.PedidosControleCozinha);
            context.Usuarios.RemoveRange(context.Usuarios);

            await context.SaveChangesAsync();

            var usuario = context.Usuarios.Add(new Usuario
            {
                Nome = "Yuri",
                Email = "yuri@email.com",
                Senha = Base64Helper.Encode("yuri"),
                Role = "ADMIN"
            });

            await context.SaveChangesAsync();

            var pedido = context.Pedidos.Add(new Pedido
            {
                DataInclusao = DateTime.Now,
                UsuarioId = 1,
                PrecoTotal = 50.00M,
                Status = StatusPedido.Pendente,
                TipoEntrega = "DELIVERY",
                Usuario = usuario.Entity
            });

            await context.SaveChangesAsync();

            context.PedidosControleCozinha.Add(new PedidoControleCozinha
            {
                DataInclusao = DateTime.Now,
                PedidoId = 1,
                NomeCliente = "Yuri",
                Status = StatusPedido.Pendente,
                Pedido = pedido.Entity
            });

            await context.SaveChangesAsync();
        }

    }
}