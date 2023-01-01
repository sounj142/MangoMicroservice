namespace PaymentProcessor;

public interface IProcessPayment
{
    Task<bool> PaymentProcessor(PaymentRequest paymentRequest);
}