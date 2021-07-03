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
            NetWorthLabel.Text = FinanceManager.NetWorth.FormatToMoney();

            ListItems.Children.Clear();

            ListItems.Children.Add(new MonthlyFinancesChartCard(DateTime.Today));
            ListItems.Children.Add(new ListMonthlyFinances(DateTime.Today));
        }
    }
}