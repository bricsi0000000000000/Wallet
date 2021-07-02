using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonthlyFinancesChartCard : ContentView
    {
        public MonthlyFinancesChartCard(List<Finance> allFinances, List<ChartEntry> expenses, DateTime date)
        {
            InitializeComponent();

            ExpensesChartFrame.BackgroundColor =
            DailyChart.BackgroundColor =
            ExpensesChart.BackgroundColor = ColorManager.Background;

            foreach (Finance finance in allFinances)
            {
                ChartLabels.Children.Add(new ChartValue(finance));
            }

            ExpensesChart.Chart = new RadialGaugeChart
            {
                Entries = expenses,
                BackgroundColor = ColorManager.BackgroundSK,
                LabelTextSize = 30,
                IsAnimated = false,
                AnimationDuration = new TimeSpan()
            };

            List<ChartEntry> finances = new List<ChartEntry>();
            for (int dayIndex = 1; dayIndex <= DateTime.DaysInMonth(date.Year, date.Month); dayIndex++)
            {
                Finance finance = allFinances.Find(x => x.Date.Day == dayIndex);
                if (finance != null)
                {
                    string color = FinanceCategoryManager.Get(finance.CategoryId).ColorCode;
                    finances.Add(new ChartEntry(finance.Money)
                    {
                        Label = dayIndex.ToString(),
                        ValueLabel = finance.Money.FormatToMoney(),
                        Color = color.ToSKColor(),
                        ValueLabelColor = color.ToSKColor()
                    });
                }
                else
                {
                    finances.Add(new ChartEntry(0)
                    {
                        Label = dayIndex.ToString(),
                        Color = ColorManager.DefaultSKColor,
                        ValueLabelColor = ColorManager.DefaultSKColor
                    });
                }
            }
            //List<Finance> montlhyFinances = FinanceManager.Finances.FindAll(x => x.Date.Year == date.Year && x.Date.Month == date.Month);
            //for (int dayIndex = 1; dayIndex <= DateTime.DaysInMonth(date.Year, date.Month); dayIndex++)
            //{
            //    List<Finance> categoryFinances = allFinances.FindAll(x => x.Date.Day == dayIndex && x.Type == FinanceType.Expense);
            //    int money = categoryFinances.Sum(x => x.Money);

            //    string color = FinanceCategoryManager.Get(categoryFinances[0].CategoryId).ColorCode;
            //    finances.Add(new ChartEntry(money)
            //    {
            //        Label = dayIndex.ToString(),
            //        ValueLabel = money.FormatToMoney(),
            //        Color = color.ToSKColor(),
            //        ValueLabelColor = color.ToSKColor()
            //    });
            //}

            DailyChart.Chart = new LineChart
            {
                Entries = finances,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Circle,
                PointSize = 18,
                BackgroundColor = ColorManager.BackgroundSK,
                EnableYFadeOutGradient = true,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Vertical,
                LabelTextSize = 30,
                IsAnimated = false
            };
        }
    }
}