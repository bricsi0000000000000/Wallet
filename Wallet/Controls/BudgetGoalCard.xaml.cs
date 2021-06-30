using System;
using Wallet.Models;
using Wallet.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BudgetGoalCard : ContentView
    {
        int id;

        private readonly Color expenseColor;
        private readonly Color income;

        public BudgetGoalCard(Budget budget)
        {
            InitializeComponent();

            id = budget.Id;

            expenseColor = (Color)Application.Current.Resources["Expense"];
            income = (Color)Application.Current.Resources["Income"];

            FinanceCategory category = FinanceCategoryManager.Get(budget.CategoryId);

            CategoryNameLabel.Text = category.Name;
            EditButton.BackgroundColor = Color.FromHex(category.ColorCode);
            SpentMoneyLabel.Text = budget.SpentMoney.FormatToMoney();
            MaxMoneyLabel.Text = budget.MaxMoney.FormatToMoney();

            float rate = (float)budget.SpentMoney / budget.MaxMoney;
            Progress.Progress = rate;

            if (budget.SpentMoney >= budget.MaxMoney)
            {
                OverMoneyLabel.Text += $" +{rate * 100:f0}%";
                Progress.ProgressColor = expenseColor;
                SpentMoneyLabel.TextColor = expenseColor;
                OverMoneyLabel.TextColor = expenseColor;
            }
            else
            {
                OverMoneyLabel.Text += $"{rate * 100:f0}%";
                OverMoneyLabel.TextColor = income;
            }
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddBudget(id));
        }
    }
}