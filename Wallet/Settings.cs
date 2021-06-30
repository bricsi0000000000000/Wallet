using System.Collections.Generic;

namespace Wallet
{
    public static class Settings
    {
        public static string AcitveCurrency;
        public static bool FirstTime = true;

        public static List<string> Currencies = new List<string>();

        public static void AddCurrency(string cultureInfo)
        {
            Currencies.Add(cultureInfo);
        }
    }
}
