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
    public partial class CalculateExpenseItemCard : ContentView
    {
        List<ChartEntry> expenses = new List<ChartEntry>();

        private readonly Color cardBackgroundColor;
        private readonly Color expenseColor;
        private readonly Color incomeColor;

        public CalculateExpenseItemCard(int income, int expense, DateTime date)
        {
            InitializeComponent();

            cardBackgroundColor = (Color)Application.Current.Resources["White"];
            incomeColor = (Color)Application.Current.Resources["Income"];
            expenseColor = (Color)Application.Current.Resources["Expense"];

            IncomeLabel.Text = income.FormatToMoney();
            IncomeLabel.TextColor = incomeColor;

            ExpensesLabel.Text = expense.FormatToMoney();
            ExpensesLabel.TextColor = expenseColor;

            TotalLabel.Text = (income - expense).FormatToMoney();

            expenses.Add(CreateChartEntry(income, false));
            expenses.Add(CreateChartEntry(expense, true));

            ExpensesChart.Chart = MakeChart();
        }

        private DonutChart MakeChart()
        {
            return new DonutChart
            {
                Entries = expenses,
                BackgroundColor = SKColor.Parse(cardBackgroundColor.ToHex()),
                LabelTextSize = 30,
                IsAnimated = true
            };
        }

        private ChartEntry CreateChartEntry(float money, bool isExpense)
        {
            return new ChartEntry(money)
            {
                Color = isExpense ? SKColor.Parse(expenseColor.ToHex()) : SKColor.Parse(incomeColor.ToHex())
            };
        }
    }
}