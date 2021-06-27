using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Wallet.Models;
using Wallet.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryItemCard : ContentView
    {
        List<ChartEntry> expenses = new List<ChartEntry>();

        private const string CARD_BACKGROUND_COLOR = "#ffffff";
        private const string RED = "#B00020";
        private const string GREEN = "#27a555";

        public HistoryItemCard(int income, int expense, DateTime date, FinanceCategory category, int mostExpenseMoney, List<Finance> finances)
        {
            InitializeComponent();

            IncomeLabel.Text = income.FormatToNumber();
            IncomeLabel.TextColor = Color.FromHex(GREEN);

            ExpensesLabel.Text = expense.FormatToNumber();
            ExpensesLabel.TextColor = Color.FromHex(RED);

            TotalLabel.Text = (income - expense).FormatToNumber();
            DateLabel.Text = date.ToString("MMM yyyy");

            MostExpensesCategoryLabel.Text = $"{category.Name} {mostExpenseMoney.FormatToNumber()}";
            MostExpensesCategoryLabel.TextColor = Color.FromHex(category.ColorCode);

            expenses.Add(CreateChartEntry(income, false));
            expenses.Add(CreateChartEntry(expense, true));

            ExpensesChart.Chart = MakeChart();

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new ShowMonth(finances));
            };
            ExpensesChart.GestureRecognizers.Add(tap);
        }

        private DonutChart MakeChart()
        {
            return new DonutChart
            {
                Entries = expenses,
                BackgroundColor = SKColor.Parse(CARD_BACKGROUND_COLOR),
                LabelTextSize = 30,
                IsAnimated = true
            };
        }

        private ChartEntry CreateChartEntry(float money, bool isExpense)
        {
            return new ChartEntry(money)
            {
                Color = isExpense ? SKColor.Parse(RED) : SKColor.Parse(GREEN)
            };
        }
    }
}