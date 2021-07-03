using SkiaSharp;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Wallet
{
    public static class ColorManager
    {
        private static Color backgroundLight;
        private static Color backgroundDark;

        private static Color textLight;
        private static Color textDark;

        private static Color textSecondaryLight;
        private static Color textSecondaryDark;

        private static Color expense;
        private static Color income;

        private static Color placeholderLight;
        private static Color placeholderDark;

        private static Color deleteButton;
        private static Color deleteButtonDark;

        private static Color gradient1Light;
        private static Color gradient1Dark;

        private static Color gradient2Light;
        private static Color gradient2Dark;

        private static Color buttonDark;

        private static List<string> chartLightColorCodes = new List<string>
        {
            "#232F34",
            "#295668",
            "#2E7A98",
            "#3292B8",
            "#35A3CF",
            "#38BAED"
        };

        private static List<string> chartDarkColorCodes = new List<string>
        {
            "#FFFFFF",
            "#C0E9F9",
            "#97DBF6",
            "#38BAED",
            "#46AAD2",
            "#2A9DCB"
        };

        public static void InitializeColors()
        {
            backgroundLight = (Color)Application.Current.Resources["White"];
            backgroundDark = (Color)Application.Current.Resources["BackgroundDarkMode"];

            textLight = (Color)Application.Current.Resources["Title"];
            textDark = (Color)Application.Current.Resources["White"];

            textSecondaryLight = (Color)Application.Current.Resources["600"];
            textSecondaryDark = (Color)Application.Current.Resources["SecondaryDarkText"];

            expense = (Color)Application.Current.Resources["Expense"];
            income = (Color)Application.Current.Resources["Income"];

            placeholderLight = (Color)Application.Current.Resources["PrimaryBackground"];
            placeholderDark = (Color)Application.Current.Resources["600"];

            deleteButton = (Color)Application.Current.Resources["Delete"];
            deleteButtonDark = (Color)Application.Current.Resources["DeleteButtonDark"];

            gradient1Light = (Color)Application.Current.Resources["Gradient1"];
            gradient1Dark = (Color)Application.Current.Resources["Gradient1Dark"];

            gradient2Light = (Color)Application.Current.Resources["Gradient2"];
            gradient2Dark = (Color)Application.Current.Resources["Gradient2Dark"];

            buttonDark = (Color)Application.Current.Resources["ButtonDark"];
        }

        public static Color Background
        {
            get
            {
                return IsDarkMode ? backgroundDark : backgroundLight;
            }
        }

        public static SKColor BackgroundSK
        {
            get
            {
                return IsDarkMode ? SKColor.Parse(backgroundDark.ToHex()) : SKColor.Parse(backgroundLight.ToHex());
            }
        }

        public static Color Expense
        {
            get
            {
                return expense;
            }
        }

        public static Color Income
        {
            get
            {
                return income;
            }
        }

        public static SKColor ExpenseOrIncomeSK(bool expense)
        {
            return expense ? SKColor.Parse(Expense.ToHex()) : SKColor.Parse(Income.ToHex());
        }

        public static Color ExpenseOrIncome(bool expense)
        {
            return expense ? Expense : Income;
        }

        public static Color Text
        {
            get
            {
                return IsDarkMode ? textDark : textLight;
            }
        }

        public static Color SecondaryText
        {
            get
            {
                return IsDarkMode ? textSecondaryDark : textSecondaryLight;
            }
        }

        public static Color PlaceholderText
        {
            get
            {
                return IsDarkMode ? placeholderDark : placeholderLight;
            }
        }

        public static Color DeleteButton
        {
            get
            {
                return IsDarkMode ? deleteButtonDark : deleteButton;
            }
        }

        public static Color EditButton
        {
            get
            {
                return Income;
            }
        }

        public static Color Button
        {
            get
            {
                return IsDarkMode ? buttonDark : Income;
            }
        }

        public static Color Gradient1
        {
            get
            {
                return IsDarkMode ? gradient1Dark : gradient1Light;
            }
        }

        public static Color Gradient2
        {
            get
            {
                return IsDarkMode ? gradient2Dark : gradient2Light;
            }
        }

        public static SKColor GetSKChartColor(int index)
        {
            if (index >= 0 && index < chartDarkColorCodes.Count)
            {
                return IsDarkMode ? chartDarkColorCodes[index].ToSKColor() : chartLightColorCodes[index].ToSKColor();
            }

            return IsDarkMode ? chartDarkColorCodes[0].ToSKColor() : chartLightColorCodes[0].ToSKColor();
        }

        public static SKColor GetSKChartColorFromBack(int index)
        {
            return GetSKChartColor(chartDarkColorCodes.Count - index - 1);
        }

        private static bool IsDarkMode
        {
            get
            {
                return Application.Current.RequestedTheme == OSAppTheme.Dark;
            }
        }

        public static Color IsInputEmpty(bool empty)
        {
            return empty ? Expense : Background;
        }

        public static SKColor DefaultSKColor
        {
            get
            {
                return IsDarkMode ? SKColor.Parse(textSecondaryDark.ToHex()) : SKColor.Parse(textSecondaryLight.ToHex());
            }
        }
    }
}
