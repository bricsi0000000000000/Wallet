using Microcharts;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculateExpenseItemCard : ContentView
    {
        private readonly List<ChartEntry> expenses = new List<ChartEntry>();

        public CalculateExpenseItemCard(int income, int expense, int expenseWithBudgets)
        {
            InitializeComponent();

            //MainFrame.BackgroundColor = ColorManager.Background;

            IncomeLabel.Text = income.FormatToMoney();
            IncomeLabel.TextColor = ColorManager.Income;

            ExpensesLabel.Text = expense.FormatToMoney();
            ExpensesLabel.TextColor = ColorManager.Expense;

            ExpensesWithBudgetGoalsLabel.Text = (expense + expenseWithBudgets).FormatToMoney();
            ExpensesWithBudgetGoalsLabel.TextColor = ColorManager.Expense;

            TotalLabel.Text = (income - expense).FormatToMoney();
            TotalWithBudgetGoalsLabel.Text = (income - (expense + expenseWithBudgets)).FormatToMoney();

            expenses.Add(CreateChartEntry(income, false));
            expenses.Add(CreateChartEntry(expense, true));

            ExpensesChart.Chart = MakeChart();
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