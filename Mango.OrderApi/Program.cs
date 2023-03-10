using Mango.OrderApi;
using Mango.OrderApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices.Config(builder);

var app = builder.Build();

await using var checkoutReceiver = app.Services.GetRequiredService<CheckoutQueueReceiver>();
await using var paymentStatusReceiver = app.Services.GetRequiredService<OrderPaymentStatusUpdatedServiceBusReceiver>();
await checkoutReceiver.Subscribe();
await paymentStatusReceiver.Subscribe();

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