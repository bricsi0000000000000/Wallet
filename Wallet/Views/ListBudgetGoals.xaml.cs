using System;
using Wallet.Controls;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListBudgetGoals : ContentPage
    {
        public ListBudgetGoals()
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
            LoadBudgetGoalFrames();
        }

        private void LoadBudgetGoalFrames()
        {
            BudgetGoalManager.Sort();
            foreach (Budget budget in BudgetGoalManager.BudgetGoals)
            {
                ListItems.Children.Add(new BudgetGoalCard(budget));
            }

            Frame emptyFrame = new Frame
            {
                CornerRadius = 5,
                HasShadow = false,
                Padding = 20,
                HeightRequest = 50,
                Opacity = 0
            };

            ListItems.Children.Add(emptyFrame);
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddBudget());
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int id = int.Parse(button.ClassId);
            BudgetGoalManager.Remove(id);
            Database.SaveBudgetGoals();
            LoadUI();
        }
    }
}