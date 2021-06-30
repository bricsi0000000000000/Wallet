using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Color cardBackgroundColor;
        private readonly Color expenseColor;
        private readonly Color incomeColor;

        public HistoryItemCard(int income, int expense, DateTime date, FinanceCategory category, List<Finance> finances)
        {
            InitializeComponent();

            cardBackgroundColor = (Color)Application.Current.Resources["White"];
            incomeColor = (Color)Application.Current.Resources["Income"];
            expenseColor = (Color)Application.Current.Resources["Expense"];

            IncomeLabel.Text = income.FormatToMoney();
            this.IncomeLabel.TextColor = expenseColor;

            ExpensesLabel.Text = expense.FormatToMoney();
            ExpensesLabel.TextColor = incomeColor;

            TotalLabel.Text = (income - expense).FormatToMoney();
            DateLabel.Text = date.ToString("MMM yyyy");

            int money = FinanceManager.Finances.FindAll(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.CategoryId == category.Id).Sum(x => x.Money);
            MostExpensesCategoryLabel.Text = $"{category.Name} {money.FormatToMoney()}";
            MostExpensesCategoryLabel.TextColor = Color.FromHex(category.ColorCode);

            expenses.Add(CreateChartEntry(income, false));
            expenses.Add(CreateChartEntry(expense, true));

            ExpensesChart.Chart = MakeChart();

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new ShowMonth(finances, date));
            };
            ExpensesChart.GestureRecognizers.Add(tap);
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