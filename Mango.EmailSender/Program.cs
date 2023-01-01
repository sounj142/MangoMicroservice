using Mango.EmailSender;
using Mango.EmailSender.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices.Config(builder);

var app = builder.Build();

await using var paymentStatusReceiver = app.Services.GetRequiredService<OrderPaymentStatusUpdatedServiceBusReceiver>();
await paymentStatusReceiver.Subscribe();

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