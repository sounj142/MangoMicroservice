using Mango.ShoppingCartApi;
using Mango.ShoppingCartApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices.Config(builder);

var app = builder.Build();

await using var productSavedReceiver = app.Services.GetRequiredService<ProductSavedServiceBusReceiver>();
await productSavedReceiver.Subscribe();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();