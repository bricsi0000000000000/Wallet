using System;
using Wallet.Models;
using Wallet.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryCard : ContentView
    {
        int id;

        public CategoryCard(FinanceCategory category)
        {
            InitializeComponent();

            this.id = category.Id;

            NameLabel.Text = category.Name;
            EditButton.BackgroundColor = Color.FromHex(category.ColorCode);
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddCategory(id));
        }
    }
}