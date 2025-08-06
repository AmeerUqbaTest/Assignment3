namespace Assignment.Strategies
{
    public interface IPaymentStrategy
    {
        bool ProcessPayment(decimal amount, Dictionary<string, string> paymentDetails);
        bool ValidatePaymentDetails(Dictionary<string, string> paymentDetails);
        string GetPaymentMethodName();
    }

    public class PaymentContext
    {
        private IPaymentStrategy _paymentStrategy;

        public PaymentContext(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public bool ProcessPayment(decimal amount, Dictionary<string, string> paymentDetails)
        {
            if (!_paymentStrategy.ValidatePaymentDetails(paymentDetails))
            {
                throw new ArgumentException("Invalid payment details");
            }

            return _paymentStrategy.ProcessPayment(amount, paymentDetails);
        }

        public string GetCurrentPaymentMethod()
        {
            return _paymentStrategy.GetPaymentMethodName();
        }
    }
}
