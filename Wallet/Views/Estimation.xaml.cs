using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Estimation : ContentPage
    {
        List<ChartEntry> after6MonthFinance = new List<ChartEntry>();
        List<ChartEntry> after1YearFinance = new List<ChartEntry>();
        List<ChartEntry> after3YearFinance = new List<ChartEntry>();

        private List<string> colorCodes = new List<string>
        {
            "#80FBFF",
            "#6CFCF3",
            "#4EFDE1",
            "#38FDD4",
            "#1FFEC5",
            "#00FFB3"
        };

        public Estimation()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            List<Finance> automatizedFinances = FinanceManager.Finances.FindAll(x => x.AutomatizedDate != null);

            // after 6 month
            after6MonthFinance.Clear();

            float money = FinanceManager.Balance;

            after6MonthFinance.Add(CreateChartEntry(money, "Today", colorCodes[0]));
            MakeEntries(after6MonthFinance, 6, money, automatizedFinances);
            After6MonthChart.Chart = CreateChart(after6MonthFinance);

            // after 1 year
            after1YearFinance.Clear();

            money = FinanceManager.Balance;

            after1YearFinance.Add(CreateChartEntry(money, "Today", colorCodes[0]));
            MakeEntries(after1YearFinance, 12, money, automatizedFinances);
            After1YearChart.Chart = CreateChart(after1YearFinance);

            // after 3 year
            after3YearFinance.Clear();

            money = FinanceManager.Balance;

            after3YearFinance.Add(CreateChartEntry(money, "Today", colorCodes[0]));
            MakeEntries(after3YearFinance, 3 * 12, money, automatizedFinances);
            After3YearChart.Chart = CreateChart(after3YearFinance);
        }

        private void MakeEntries(List<ChartEntry> finances, int monthes, float money, List<Finance> automatizedFinances)
        {
            int entriIndex = 0;
            for (int i = 0; i < monthes; i++)
            {
                foreach (Finance finance in automatizedFinances)
                {
                    if (finance.IsExpense)
                    {
                        money -= finance.Money;
                    }
                    else
                    {
                        money += finance.Money;
                    }
                }

                bool addEntry = false;
                if (i > 0)
                {
                    if (monthes <= 6)
                    {
                        addEntry = true;
                    }
                    else if (monthes == 12)
                    {
                        if (i % 3 == 0 || i == monthes - 1)
                        {
                            addEntry = true;
                        }
                    }
                    else if (monthes == 3 * 12)
                    {
                        if (i % 9 == 0 || i == monthes - 1)
                        {
                            addEntry = true;
                        }
                    }
                }

                if (addEntry)
                {
                    finances.Add(CreateChartEntry(money, $"{i + 1}", colorCodes[entriIndex++]));
                }
            }
        }

        private LineChart CreateChart(List<ChartEntry> finances)
        {
            return new LineChart
            {
                Entries = finances,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Circle,
                PointSize = 18,
                BackgroundColor = SKColor.Parse("#303030"),
                EnableYFadeOutGradient = true,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30
            };
        }

        private ChartEntry CreateChartEntry(float money, string label, string colorCode)
        {
            money /= 1000000;
            money = (float)decimal.Round((decimal)money, 2, MidpointRounding.AwayFromZero);

            return new ChartEntry(money)
            {
                Label = label,
                ValueLabel = $"{money} M",
                ValueLabelColor = SKColor.Parse(colorCode),
                Color = SKColor.Parse(colorCode)
            };
        }
    }
}