using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListCategories : ContentPage
    {
        private const string BACKGROUND_COLOR = "#ffffff";
        private const string TEXT_COLOR = "#344955";
        private const string RED = "#B00020";

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
                Frame categoryFrame = new Frame
                {
                    CornerRadius = 5,
                    HasShadow = true,
                    Padding = 20
                };

                Grid grid = new Grid();

                ColumnDefinition columnDefinition1 = new ColumnDefinition();

                ColumnDefinition columnDefinition2 = new ColumnDefinition
                {
                    Width = new GridLength(50, GridUnitType.Absolute)
                };

                Label categoryNameLabel = new Label()
                {
                    Text = category.Name,
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex(TEXT_COLOR)
                };

                TapGestureRecognizer moneyLabelTap = new TapGestureRecognizer();
                moneyLabelTap.Tapped += (s, e) =>
                {
                    Navigation.PushAsync(new AddCategory(category.Id));
                };
                categoryNameLabel.GestureRecognizers.Add(moneyLabelTap);

                Button deleteButton = new Button
                {
                    ClassId = category.Id.ToString(),
                    Text = "✖",
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Color.FromHex(RED)
                };

                categoryFrame.BackgroundColor = Color.FromHex(BACKGROUND_COLOR);
                categoryFrame.BorderColor = Color.FromHex(category.ColorCode);
                deleteButton.BackgroundColor = Color.FromHex(BACKGROUND_COLOR);

                deleteButton.Clicked += Delete_Clicked;

                grid.ColumnDefinitions.Add(columnDefinition1);
                grid.ColumnDefinitions.Add(columnDefinition2);

                grid.Children.Add(categoryNameLabel);
                grid.Children.Add(deleteButton);

                categoryNameLabel.SetValue(Grid.ColumnProperty, 0);
                deleteButton.SetValue(Grid.ColumnProperty, 1);

                categoryFrame.Content = grid;
                ListItems.Children.Add(categoryFrame);
            }
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