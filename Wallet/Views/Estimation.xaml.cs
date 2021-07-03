using System;
using System.Collections.Generic;
using System.Linq;
using Wallet.Controls;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Estimation : ContentPage
    {
        public Estimation()
        {
            InitializeComponent();

            ExpenseFrame.BackgroundColor =
            IncomeFrame.BackgroundColor =
            IncomeInput.BackgroundColor = 
            ExpenseInput.BackgroundColor = ColorManager.Background;

            ExpenseInput.TextColor = ColorManager.Text;
            ExpenseInput.PlaceholderColor = ColorManager.PlaceholderText;

            IncomeInput.TextColor = ColorManager.Text;
            IncomeInput.PlaceholderColor = ColorManager.PlaceholderText;
        }

        protected override void OnAppearing()
        {
            Calculate();
        }

        private void Calculate()
        {
            if (!FinanceManager.Finances.Any())
            {
                return;
            }

            ListItems.Children.Clear();

            List<Finance> automatizedFinances = FinanceManager.Finances.FindAll(x => x.IsAutomatized);

            if (!string.IsNullOrEmpty(ExpenseInput.Text))
            {
                Finance expenseFinance = new Finance
                {
                    Money = int.Parse(ExpenseInput.Text),
                    Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 10),
                    Type = FinanceType.Expense
                };
                automatizedFinances.Add(expenseFinance);
            }

            if (!string.IsNullOrEmpty(IncomeInput.Text))
            {
                Finance incomeFinance = new Finance
                {
                    Money = int.Parse(IncomeInput.Text),
                    Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 10),
                    Type = FinanceType.Income
                };
                automatizedFinances.Add(incomeFinance);
            }

            int income = 0;
            int expense = 0;
            foreach (Finance item in automatizedFinances)
            {
                if (item.Type == FinanceType.Income)
                {
                    income += item.Money;
                }
                if (item.Type == FinanceType.Expense)
                {
                    expense += item.Money;
                }
            }

            ListItems.Children.Add(new CalculateExpenseItemCard(income, expense));

            ListItems.Children.Add(new EstimationChartCard(FinanceManager.NetWorth, 6, automatizedFinances));
            ListItems.Children.Add(new EstimationChartCard(FinanceManager.NetWorth, 12, automatizedFinances));
            ListItems.Children.Add(new EstimationChartCard(FinanceManager.NetWorth, 3 * 12, automatizedFinances));
        }

        private void Calculate_Clicked(object sender, EventArgs e)
        {
            Calculate();
        }
    }
}