namespace MovieRental.PaymentProviders
{
    public interface IPaymentProvider
    {
        Task<bool> Pay(double price);
    }
}