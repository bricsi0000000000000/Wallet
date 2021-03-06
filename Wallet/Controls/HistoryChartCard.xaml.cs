using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Wallet.Models;
using System.Linq;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryChartCard : ContentView
    {
        private readonly Color cardBackgroundColor;
        List<ChartEntry> expenses = new List<ChartEntry>();
        private List<string> colorCodes = new List<string>
        {
            "#232F34",
            "#295668",
            "#2E7A98",
            "#3292B8",
            "#35A3CF",
            "#38BAED"
        };

        public HistoryChartCard()
        {
            InitializeComponent();

            cardBackgroundColor = (Color)Application.Current.Resources["White"];
            int index = 0;

            foreach (IGrouping<(int Year, int Month), Finance> group in FinanceManager.Finances.GroupBy(x => (x.Date.Year, x.Date.Month)))
            {
                DateTime date = group.First().Date;

                if (date.Month >= DateTime.Today.Month - 6)
                {
                    int money = 0;
                    foreach (Finance finance in group)
                    {
                        if (finance.Type == FinanceType.Expense)
                        {
                            money += finance.Money;
                        }
                    }

                    expenses.Insert(0, CreateChartEntry(money, date.FormatToMonth(), colorCodes[colorCodes.Count - index - 1]));

                    index++;
                }
            }

            Chart.Chart = CreateChart();
        }

        private LineChart CreateChart()
        {
            return new LineChart
            {
                Entries = expenses,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Circle,
                PointSize = 18,
                BackgroundColor = SKColor.Parse(cardBackgroundColor.ToHex()),
                EnableYFadeOutGradient = true,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30,
                IsAnimated = false
            };
        }

        private ChartEntry CreateChartEntry(int money, string label, string colorCode)
        {
            return new ChartEntry(money)
            {
                Label = label,
                ValueLabel = money.FormatToMoney(),
                ValueLabelColor = SKColor.Parse(colorCode),
                Color = SKColor.Parse(colorCode)
            };
        }
    }
}