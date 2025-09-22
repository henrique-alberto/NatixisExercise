namespace MovieRental.PaymentProviders
{
    public class MbWayProvider : IPaymentProvider
    {
        public Task<bool> Pay(double price)
        {
            return Task.FromResult<bool>(true);
        }
    }
}