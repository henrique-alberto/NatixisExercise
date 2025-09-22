namespace MovieRental.PaymentProviders
{
    public class PayPalProvider : IPaymentProvider
    {
        public Task<bool> Pay(double price)
        {
            return Task.FromResult<bool>(true);
        }
    }
}