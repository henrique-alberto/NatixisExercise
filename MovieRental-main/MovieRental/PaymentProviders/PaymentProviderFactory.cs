namespace MovieRental.PaymentProviders
{
    public interface IPaymentProviderFactory
    {
        IPaymentProvider GetPaymentProvider(string paymentMethod);
    }

    //TODO use dependency to news provider?
    public class PaymentProviderFactory : IPaymentProviderFactory
    {
        public IPaymentProvider GetPaymentProvider(string paymentMethod)
        {
            return paymentMethod.ToLower() switch
            {
                "paypal" => new PayPalProvider(),
                "mbway" => new MbWayProvider(),
                _ => throw new ArgumentException($"Payment provider '{paymentMethod}' not supported.")
            };
        }
    }
}