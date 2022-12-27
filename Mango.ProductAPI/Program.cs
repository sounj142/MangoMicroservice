using Mango.ProductAPI;
using Mango.ProductAPI.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices.Config(builder);

var app = builder.Build();

SeedDatabase.InitializeDatabase(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();