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
        private readonly List<ChartEntry> expenses = new List<ChartEntry>();

        public HistoryItemCard(int income, int expense, DateTime date, FinanceCategory category, List<Finance> finances)
        {
            InitializeComponent();

            MainFrame.BackgroundColor = ColorManager.Background;

            IncomeTitleLabel.TextColor =
            ExpenseTitleLabel.TextColor =
            TotalTitleLabel.TextColor = ColorManager.SecondaryText;

            DateLabel.TextColor = ColorManager.Text;
            DateLabel.Text = date.FormatToMonthYear();

            IncomeLabel.Text = income.FormatToMoney();
            IncomeLabel.TextColor = ColorManager.Income;

            ExpensesLabel.Text = expense.FormatToMoney();
            ExpensesLabel.TextColor = ColorManager.Expense;

            TotalLabel.Text = (income - expense).FormatToMoney();
            TotalLabel.TextColor = ColorManager.Text;

            int money = FinanceManager.Finances.FindAll(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.CategoryId == category.Id).Sum(x => x.Money);
            MostExpensesCategoryLabel.Text = $"{category.Name} {money.FormatToMoney()}";
            MostExpensesCategoryLabel.TextColor = category.ColorCode.ToColor();

            if (income > 0)
            {
                expenses.Add(CreateChartEntry(income, false));
            }
            if (expense > 0)
            {
                expenses.Add(CreateChartEntry(expense, true));
            }

            ExpensesChart.Chart = MakeChart();

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new ShowMonth(date));
            };
            ExpensesChart.GestureRecognizers.Add(tap);
        }

        private DonutChart MakeChart()
        {
            return new DonutChart
            {
                Entries = expenses,
                BackgroundColor = ColorManager.BackgroundSK,
                LabelTextSize = 30,
                IsAnimated = true
            };
        }

        private ChartEntry CreateChartEntry(float money, bool expense)
        {
            return new ChartEntry(money)
            {
                Color = ColorManager.ExpenseOrIncomeSK(expense)
            };
        }
    }
}