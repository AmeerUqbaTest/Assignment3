using Assignment.Singletons;

namespace Assignment.Strategies
{
    public class CryptoPayment : IPaymentStrategy
    {
        public bool ProcessPayment(decimal amount, Dictionary<string, string> paymentDetails)
        {
            Logger.Instance.LogInfo($"Processing cryptocurrency payment of ${amount:F2}");
            
            var walletAddress = paymentDetails["walletAddress"];
            var cryptoType = paymentDetails["cryptoType"];
            var maskedWallet = $"{walletAddress.Substring(0, 6)}...{walletAddress.Substring(walletAddress.Length - 6)}";
            
            // Simulate payment processing
            var random = new Random();
            bool success = random.NextDouble() > 0.3; // 70% success rate (crypto can be volatile)
            
            if (success)
            {
                Logger.Instance.LogInfo($"{cryptoType} payment successful for wallet {maskedWallet}");
                return true;
            }
            else
            {
                Logger.Instance.LogError($"{cryptoType} payment failed for wallet {maskedWallet}");
                return false;
            }
        }

        public bool ValidatePaymentDetails(Dictionary<string, string> paymentDetails)
        {
            if (!paymentDetails.ContainsKey("walletAddress") || 
                !paymentDetails.ContainsKey("cryptoType"))
            {
                return false;
            }

            var walletAddress = paymentDetails["walletAddress"];
            var cryptoType = paymentDetails["cryptoType"];

            // Basic wallet address validation (simplified)
            if (string.IsNullOrEmpty(walletAddress) || walletAddress.Length < 26 || walletAddress.Length > 62)
                return false;

            // Check if it's a valid crypto type
            var validCryptoTypes = new[] { "Bitcoin", "Ethereum", "Litecoin", "Dogecoin" };
            if (!validCryptoTypes.Contains(cryptoType, StringComparer.OrdinalIgnoreCase))
                return false;

            return true;
        }

        public string GetPaymentMethodName()
        {
            return "Cryptocurrency";
        }
    }
}
