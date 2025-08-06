using Assignment.Singletons;

namespace Assignment.Strategies
{
    public class BankTransferPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount, Dictionary<string, string> paymentDetails)
        {
            Logger.Instance.LogInfo($"Processing bank transfer payment of ${amount:F2}");
            
            var accountNumber = paymentDetails["accountNumber"];
            var maskedAccount = $"****{accountNumber.Substring(accountNumber.Length - 4)}";
            
            // Simulate payment processing
            var random = new Random();
            bool success = random.NextDouble() > 0.25; // 75% success rate (bank transfers can be slower/more complex)
            
            if (success)
            {
                Logger.Instance.LogInfo($"Bank transfer payment successful for account {maskedAccount}");
                return true;
            }
            else
            {
                Logger.Instance.LogError($"Bank transfer payment failed for account {maskedAccount}");
                return false;
            }
        }

        public bool ValidatePaymentDetails(Dictionary<string, string> paymentDetails)
        {
            if (!paymentDetails.ContainsKey("routingNumber") || 
                !paymentDetails.ContainsKey("accountNumber") ||
                !paymentDetails.ContainsKey("accountHolderName"))
            {
                return false;
            }

            var routingNumber = paymentDetails["routingNumber"];
            var accountNumber = paymentDetails["accountNumber"];
            var accountHolderName = paymentDetails["accountHolderName"];

            // Basic validation
            if (routingNumber.Length != 9 || !routingNumber.All(char.IsDigit))
                return false;

            if (accountNumber.Length < 8 || accountNumber.Length > 17 || !accountNumber.All(char.IsDigit))
                return false;

            if (string.IsNullOrEmpty(accountHolderName) || accountHolderName.Length < 2)
                return false;

            return true;
        }

        public string GetPaymentMethodName()
        {
            return "Bank Transfer";
        }
    }
}
