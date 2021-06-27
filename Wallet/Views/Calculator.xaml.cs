using Microcharts;
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
    public partial class Calculator : ContentPage
    {
        private const string RED = "#B00020";
        private const string BACKGROUND = "#ffffff";

        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculate_Clicked(object sender, EventArgs e)
        {
            ExpenseFrame.BorderColor = string.IsNullOrEmpty(ExpenseInput.Text) ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);

            if (!string.IsNullOrEmpty(ExpenseInput.Text))
            {
                ListItems.Children.Clear();

                int expenses = int.Parse(ExpenseInput.Text);

                List<Finance> automatizedFinances = FinanceManager.Finances.FindAll(x => x.IsAutomatized);
                Finance expenseFinance = new Finance
                {
                    Money = expenses,
                    Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 10),
                    Type = FinanceType.Expense
                };
                automatizedFinances.Add(expenseFinance);

                if (!FinanceManager.Finances.Any())
                {
                    return;
                }

                float money = FinanceManager.Balance;

                ListItems.Children.Add(new EstimationChartCard(FinanceManager.Balance, automatizedFinances));

                int income = 0;
                int expense = 0;
                foreach (var item in automatizedFinances)
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

                ListItems.Children.Add(new CalculateExpenseItemCard(income, expense, DateTime.Today));
            }
        }
    }
}