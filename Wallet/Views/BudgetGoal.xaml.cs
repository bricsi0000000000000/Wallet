using System;
using Wallet.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BudgetGoal : ContentPage
    {
        public BudgetGoal()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            LoadUI();
        }

        private void LoadUI()
        {
            ListItems.Children.Clear();
            ListItems.Children.Add(new BudgetGoalCard(new Models.Budget { Id = 1, CategoryId = 2, MaxMoney = 1000, SpentMoney = 346 }));
            ListItems.Children.Add(new BudgetGoalCard(new Models.Budget { Id = 2, CategoryId = 5, MaxMoney = 1500, SpentMoney = 876 }));
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddBudget(-1));
        }
    }
}