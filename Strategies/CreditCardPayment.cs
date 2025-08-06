using Assignment.Singletons;

namespace Assignment.Strategies
{
    public class CreditCardPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount, Dictionary<string, string> paymentDetails)
        {
            Logger.Instance.LogInfo($"Processing credit card payment of ${amount:F2}");
            
            // Simulate payment processing
            var cardNumber = paymentDetails["cardNumber"];
            var maskedCard = $"****-****-****-{cardNumber.Substring(cardNumber.Length - 4)}";
            
            // Simulate random success/failure (80% success rate)
            var random = new Random();
            bool success = random.NextDouble() > 0.2;
            
            if (success)
            {
                Logger.Instance.LogInfo($"Credit card payment successful for card {maskedCard}");
                return true;
            }
            else
            {
                Logger.Instance.LogError($"Credit card payment failed for card {maskedCard}");
                return false;
            }
        }

        public bool ValidatePaymentDetails(Dictionary<string, string> paymentDetails)
        {
            if (!paymentDetails.ContainsKey("cardNumber") || 
                !paymentDetails.ContainsKey("expiryDate") || 
                !paymentDetails.ContainsKey("cvv") ||
                !paymentDetails.ContainsKey("cardHolderName"))
            {
                return false;
            }

            var cardNumber = paymentDetails["cardNumber"];
            var cvv = paymentDetails["cvv"];
            var expiryDate = paymentDetails["expiryDate"];

            // Basic validation
            if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
                return false;

            if (cvv.Length != 3 || !cvv.All(char.IsDigit))
                return false;

            if (!DateTime.TryParseExact(expiryDate, "MM/yy", null, System.Globalization.DateTimeStyles.None, out var expiry))
                return false;

            if (expiry < DateTime.Now)
                return false;

            return true;
        }

        public string GetPaymentMethodName()
        {
            return "Credit Card";
        }
    }
}
