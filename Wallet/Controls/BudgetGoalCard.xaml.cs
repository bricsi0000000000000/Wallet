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
        private readonly int id;

        public BudgetGoalCard(Budget budget)
        {
            InitializeComponent();

            id = budget.Id;

            MainFrame.BackgroundColor = ColorManager.Background;

            FinanceCategory category = FinanceCategoryManager.Get(budget.CategoryId);

            CategoryNameLabel.Text = category.Name;

            CategoryNameLabel.TextColor =
            OverMoneyLabel.TextColor =
            SpentMoneyLabel.TextColor =
            MaxMoneyLabel.TextColor = ColorManager.Text;

            EditButton.BackgroundColor = category.ColorCode.ToColor();

            SpentMoneyLabel.Text = budget.SpentMoney.FormatToMoney();
            MaxMoneyLabel.Text = budget.MaxMoney.FormatToMoney();

            float rate = (float)budget.SpentMoney / budget.MaxMoney;
            Progress.Progress = rate;

            if (budget.SpentMoney >= budget.MaxMoney)
            {
                OverMoneyLabel.Text += $" +{rate * 100:f0}%";
                Progress.ProgressColor = ColorManager.Expense;
                SpentMoneyLabel.TextColor = ColorManager.Expense;
                OverMoneyLabel.TextColor = ColorManager.Expense;
            }
            else
            {
                OverMoneyLabel.Text += $"{rate * 100:f0}%";
                OverMoneyLabel.TextColor = ColorManager.Income;
            }
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddBudget(id));
        }
    }
}