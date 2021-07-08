using SkiaSharp;
using System;
using System.Collections.Generic;
using Wallet.Models;
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

        public static string FormatToFullMonth(this DateTime date)
        {
            return date.ToString("MMMM");
        }

        public static string FormatToDay(this DateTime date)
        {
            return date.ToString("dd");
        }

        public static Color ToColor(this string colorCode)
        {
            return Color.FromHex(colorCode);
        }

        public static SKColor ToSKColor(this string colorCode)
        {
            return SKColor.Parse(colorCode);
        }

        public static List<Finance> SortList(this List<Finance> finances, bool descending = false)
        {
            for (int j = 0; j < finances.Count - 1; j++)
            {
                for (int i = 0; i < finances.Count - 1; i++)
                {
                    if (descending)
                    {
                        if (finances[i].Money < finances[i + 1].Money)
                        {
                            Finance temp = finances[i + 1];
                            finances[i + 1] = finances[i];
                            finances[i] = temp;
                        }
                    }
                    else
                    {
                        if (finances[i].Money > finances[i + 1].Money)
                        {
                            Finance temp = finances[i + 1];
                            finances[i + 1] = finances[i];
                            finances[i] = temp;
                        }
                    }
                }
            }

            return finances;
        }
    }
}
