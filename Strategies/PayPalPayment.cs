using Assignment.Singletons;

namespace Assignment.Strategies
{
    public class PayPalPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount, Dictionary<string, string> paymentDetails)
        {
            Logger.Instance.LogInfo($"Processing PayPal payment of ${amount:F2}");
            
            var email = paymentDetails["email"];
            
            // Simulate payment processing
            var random = new Random();
            bool success = random.NextDouble() > 0.15; // 85% success rate
            
            if (success)
            {
                Logger.Instance.LogInfo($"PayPal payment successful for {email}");
                return true;
            }
            else
            {
                Logger.Instance.LogError($"PayPal payment failed for {email}");
                return false;
            }
        }

        public bool ValidatePaymentDetails(Dictionary<string, string> paymentDetails)
        {
            if (!paymentDetails.ContainsKey("email") || 
                !paymentDetails.ContainsKey("password"))
            {
                return false;
            }

            var email = paymentDetails["email"];
            var password = paymentDetails["password"];

            // Basic email validation
            if (string.IsNullOrEmpty(email) || !email.Contains("@") || !email.Contains("."))
                return false;

            // Basic password validation
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return false;

            return true;
        }

        public string GetPaymentMethodName()
        {
            return "PayPal";
        }
    }
}
