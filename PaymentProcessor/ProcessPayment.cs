namespace PaymentProcessor;

public class ProcessPayment : IProcessPayment
{
    public Task<bool> PaymentProcessor(PaymentRequest paymentRequest)
    {
        return Task.FromResult(true);
    }
}