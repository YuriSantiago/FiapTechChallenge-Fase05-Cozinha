using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using CozinhaConsumidor;
using CozinhaConsumidor.Eventos;
using Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var configuration = builder.Configuration;

var queueAtualizacaoStatusPedido = configuration.GetSection("MassTransit:Queues")["PedidoAtualizacaoStatusQueue"] ?? string.Empty;

builder.Services.AddScoped<IPedidoControleCozinhaService, PedidoControleCozinhaService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoControleCozinhaRepository, PedidoControleCozinhaRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("ConnectionString"),
        sql => sql.EnableRetryOnFailure());
}, ServiceLifetime.Scoped);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PedidoStatusAtualizado>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(configuration.GetSection("MassTransit")["Server"], "/", h =>
        {
            h.Username(configuration.GetSection("MassTransit")["User"]);
            h.Password(configuration.GetSection("MassTransit")["Password"]);
        });

        cfg.ReceiveEndpoint(queueAtualizacaoStatusPedido, e =>
        {
            e.ConfigureDefaultDeadLetterTransport();
            e.ConfigureConsumer<PedidoStatusAtualizado>(context);
        });

        cfg.ConfigureEndpoints(context);
    });

});

var host = builder.Build();
host.Run();
