using System.Collections.Generic;
using Wallet.Models;

namespace Wallet
{
    public static class SettingsManager
    {
        public static Currency AcitveCurrency;
        public static bool FirstTime = true;

        public static List<Currency> Currencies = new List<Currency>();

        public static void AddCurrency(Currency currency)
        {
            Currencies.Add(currency);
        }
    }
}
