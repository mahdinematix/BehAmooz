namespace _01_Framework.Application;

public class TransactionTypes
{
    public const int Deposit = 1;
    public const int Withdraw = 2;
    public const int PayFromWallet = 3;
    public const int PayFromGateway = 4;

    public static string GetTypeById(int id)
    {
        switch (id)
        {
            case 1:
                return "شارژ";
            case 2:
                return "برداشت";
            case 3:
                return "پرداخت با کیف پول";
            case 4:
                return "پرداخت مستقیم";
            default:
                return "";

        }
    }
}
