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

        public BudgetGoalCard(Budget budget)
        {
            InitializeComponent();

            id = budget.Id;

            FinanceCategory category = FinanceCategoryManager.Get(budget.CategoryId);

            CategoryNameLabel.Text = category.Name;
            EditButton.BackgroundColor = Color.FromHex(category.ColorCode);
            CurrentMoneyLabel.Text = budget.SpentMoney.FormatToMoney();
            MaxMoneyLabel.Text = budget.MaxMoney.FormatToMoney();

            float rate = (float)budget.SpentMoney / budget.MaxMoney;
            Progress.Progress =  rate;
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddFinance(id));
        }
    }
}