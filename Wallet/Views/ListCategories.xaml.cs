using System;
using Wallet.Controls;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListCategories : ContentPage
    {
        private const string BACKGROUND_COLOR = "#EBEEF0";

        public ListCategories()
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
            LoadCategoryFrames();
        }

        private void LoadCategoryFrames()
        {
            foreach (FinanceCategory category in FinanceCategoryManager.Categories)
            {
                ListItems.Children.Add(new CategoryCard(category));
            }

            Frame emptyFrame = new Frame
            {
                CornerRadius = 5,
                HasShadow = false,
                Padding = 20,
                HeightRequest = 50,
                Opacity=0
            };

            ListItems.Children.Add(emptyFrame);
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCategory(-1));
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int id = int.Parse(button.ClassId);
            FinanceCategoryManager.Remove(id);
            Database.SaveCategories();
            LoadUI();
        }
    }
}