using System;
using System.Xml;
using Wallet.Models;
using Wallet.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryCard : ContentView
    {
        private readonly int id;

        public CategoryCard(FinanceCategory category)
        {
            InitializeComponent();

            this.id = category.Id;

            //MainFrame.BackgroundColor = ColorManager.Background;

            NameLabel.Text = category.Name;
            //NameLabel.TextColor = ColorManager.Text;

            EditButton.BackgroundColor = category.ColorCode.ToColor();
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddCategory(id));
        }
    }
}