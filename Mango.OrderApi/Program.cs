using Mango.OrderApi;
using Mango.OrderApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices.Config(builder);

var app = builder.Build();

await using var receiver = app.Services.GetRequiredService<CheckoutMessageBusReceiver>();
await receiver.Subscribe();

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