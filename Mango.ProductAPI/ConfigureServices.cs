using Mango.ProductAPI.DbContexts;
using Mango.ProductAPI.Mappers;
using Mango.ProductAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace Mango.ProductAPI;

public static class ConfigureServices
{
    public static void Config(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddScoped<IProductService, ProductService>();
    }
}