using System;

namespace Wallet
{
    public static class Extensions
    {
        public static string FormatToMoney(this int money)
        {
            return money.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("hu-HU"));
        }

        public static string FormatToDate(this DateTime date)
        {
            return date.ToString("MMM dd, yyyy");
        }
    }
}
