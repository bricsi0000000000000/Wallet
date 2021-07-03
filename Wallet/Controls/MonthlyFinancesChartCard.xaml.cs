using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonthlyFinancesChartCard : ContentView
    {
        public MonthlyFinancesChartCard(DateTime date)
        {
            InitializeComponent();

            ExpensesChart.BackgroundColor = ColorManager.Background;

            List<ChartEntry> expenses = new List<ChartEntry>();
            List<Finance> groupedFinances = FinanceManager.GetMonthlyFinance(date).GroupedFinances;

            groupedFinances.SortList(descending: false);

            foreach (Finance finance in groupedFinances)
            {
                expenses.Add(new ChartEntry(finance.Money)
                {
                    Color = FinanceCategoryManager.Get(finance.CategoryId).ColorCode.ToSKColor()
                });
            }

            ExpensesChart.Chart = new RadialGaugeChart
            {
                Entries = expenses,
                BackgroundColor = ColorManager.BackgroundSK,
                LabelTextSize = 30,
                IsAnimated = false,
                AnimationDuration = new TimeSpan()
            };

            groupedFinances.SortList(descending: true);

            foreach (Finance finance in groupedFinances)
            {
                ChartLabels.Children.Add(new ChartValue(finance));
            }
        }
    }
}