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
        private const string CARD_BACKGROUND_COLOR = "#ffffff";
        private const string TEXT_COLOR = "#344955";
        private const string PRIMARY_DARK = "#232F34";

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
                BackgroundColor = Color.FromHex(BACKGROUND_COLOR)
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