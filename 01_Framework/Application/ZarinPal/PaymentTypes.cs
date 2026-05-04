namespace _01_Framework.Application.ZarinPal
{
    public class PaymentTypes
    {
        public static int BuyFromGateway = 1;
        public static int ChargeWallet = 2;

        public static string GetType(int type)
        {
            switch (type)
            {
                case 1:
                    return "Checkout";

                case 2:
                    return "Wallet";

                default:
                    return "";
            }
        }
    }
}
