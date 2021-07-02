using SkiaSharp;
using System;
using Xamarin.Forms;

namespace Wallet
{
    public static class Extensions
    {
        public static string FormatToMoney(this int money)
        {
            return money.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo(SettingsManager.AcitveCurrency.Value));
        }

        public static string FormatToMoney(this float money)
        {
            return money.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo(SettingsManager.AcitveCurrency.Value));
        }

        public static string FormatToDate(this DateTime date)
        {
            return date.ToString("MMM dd, yyyy");
        }

        public static string FormatToMonthYear(this DateTime date)
        {
            return date.ToString("MMM, yyyy");
        }

        public static string FormatToMonth(this DateTime date)
        {
            return date.ToString("MMM");
        }

        public static Color ToColor(this string colorCode)
        {
            return Color.FromHex(colorCode);
        }

        public static SKColor ToSKColor(this string colorCode)
        {
            return SKColor.Parse(colorCode);
        }
    }
}
