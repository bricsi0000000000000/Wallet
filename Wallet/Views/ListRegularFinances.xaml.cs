using System;
using Wallet.Controls;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListRegularFinances : ContentPage
    {
        public ListRegularFinances()
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
            LoadTemplateFrames();
        }

        private void LoadTemplateFrames()
        {
            foreach (Finance finance in FinanceManager.RegularFinances)
            {
                ListItems.Children.Add(new FinanceCard(finance));
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
            await Navigation.PushAsync(new AddFinance());
        }
    }
}