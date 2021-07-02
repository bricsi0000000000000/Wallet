using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstimationChartCard : ContentView
    {
        private readonly List<ChartEntry> afterFinance = new List<ChartEntry>();

        public EstimationChartCard(int money, int monthes, List<Finance> automatizedFinances)
        {
            InitializeComponent();

            MainFrame.BackgroundColor = ColorManager.Background;
            AfterTimeLabel.TextColor = ColorManager.Text;
            Chart.BackgroundColor = ColorManager.Background;

            afterFinance.Add(CreateChartEntry(money, "Today", ColorManager.GetSKChartColor(0)));
            MakeEntries(afterFinance, monthes, money, automatizedFinances);
            Chart.Chart = CreateChart(afterFinance);

            if (monthes < 12)
            {
                AfterTimeLabel.Text = $"After {monthes} monthes";
            }
            else if (monthes == 12)
            {
                AfterTimeLabel.Text = $"After {monthes / 12} year";
            }
            else
            {
                AfterTimeLabel.Text = $"After {monthes / 12} years";
            }
        }

        private void MakeEntries(List<ChartEntry> finances, int monthes, int money, List<Finance> automatizedFinances)
        {
            int entriIndex = 0;
            for (int i = 0; i < monthes; i++)
            {
                foreach (Finance finance in automatizedFinances)
                {
                    if (finance.Type == FinanceType.Expense)
                    {
                        money -= finance.Money;
                    }
                    else if (finance.Type == FinanceType.Income)
                    {
                        money += finance.Money;
                    }
                    else
                    {
                        continue;
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
                    finances.Add(CreateChartEntry(money, $"{i + 1}", ColorManager.GetSKChartColor(entriIndex++)));
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
                BackgroundColor = ColorManager.BackgroundSK,
                EnableYFadeOutGradient = true,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 30,
                IsAnimated = false
            };
        }

        private ChartEntry CreateChartEntry(float money, string label, SKColor color)
        {
            string valueLabel;
            if (SettingsManager.AcitveCurrency.Id == 3)
            {
                money /= 1000000;
                money = (float)decimal.Round((decimal)money, 2, MidpointRounding.AwayFromZero);
                valueLabel = $"{money} M";
            }
            else
            {
                valueLabel = money.FormatToMoney();
            }

            return new ChartEntry(money)
            {
                Label = label,
                ValueLabel = valueLabel,
                ValueLabelColor = color,
                Color = color
            };
        }
    }
}