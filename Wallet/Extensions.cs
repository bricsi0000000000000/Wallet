namespace Wallet
{
    public static class Extensions
    {
        public static string FormatToNumber(this int money)
        {
            return money.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("hu-HU"));
        }
    }
}
