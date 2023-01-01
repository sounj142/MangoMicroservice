using Azure.Messaging.ServiceBus;
using Commons.Services;
using Mango.EmailSender.Mappers;
using Mango.EmailSender.Repositories;
using Mango.EmailSender.Services;
using Microsoft.EntityFrameworkCore;

namespace Mango.EmailSender;

public class ConfigureServices
{
    public static void Config(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });

        services.AddAutoMapper(typeof(MappingProfiles).Assembly);

        services.AddHttpContextAccessor();
        ConfigureAzureServiceBus(builder);

        services.AddScoped<ICurrentUserContext, CurrentUserContext>();
        services.AddScoped<IEmailService, EmailService>();
    }

    private static void ConfigureAzureServiceBus(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddSingleton(provider => new ServiceBusClient(
                    builder.Configuration["AzureServiceBus:ConnectionString"]));

        services.AddSingleton(provider => new OrderPaymentStatusUpdatedServiceBusReceiver(
            provider,
            builder.Configuration["AzureServiceBus:OrderPaymentStatusUpdatedTopic"],
            builder.Configuration["AzureServiceBus:EmailSenderSubscriptionName"]));
    }
}