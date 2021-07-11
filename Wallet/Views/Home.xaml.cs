using System;
using Wallet.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            MonthLabel.Text = DateTime.Today.FormatToMonth();
            DayLabel.Text = DateTime.Today.FormatToDay();
            MonthlyBudgetLabel.Text = BudgetGoalManager.LeftBudget.FormatToMoney();
            int leftBudget = BudgetGoalManager.LeftBudget;
            if (leftBudget <= 0)
            {
                MonthlyBudgetLabel.TextColor = ColorManager.DeleteButton;
            }
            else if (leftBudget > 0 && leftBudget <= BudgetGoalManager.AllBudget * .2)
            {
                MonthlyBudgetLabel.TextColor = ColorManager.Warning;
            }
            else
            {
                MonthlyBudgetLabel.TextColor = ColorManager.Income;
            }

            NetWorthLabel.Text = FinanceManager.NetWorth.FormatToMoney();

            ListItems.Children.Clear();

            MonthlyFinancesChartCard monthlyFinances = new MonthlyFinancesChartCard(DateTime.Today);
            ListMonthlyFinances listFinances = new ListMonthlyFinances(DateTime.Today);

            monthlyFinances.SetValue(Grid.RowProperty, 0);
            listFinances.SetValue(Grid.RowProperty, 1);

            ListItems.Children.Add(monthlyFinances);
            ListItems.Children.Add(listFinances);
        }

        private async void AddFinance(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddFinance());
        }
    }
}