using System;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategory : ContentPage
    {
        private int id;

        private const string RED = "#B00020";
        private const string BACKGROUND = "#EBEEF0";

        private bool changeColorCode = false;

        public AddCategory(int id)
        {
            InitializeComponent();

            this.id = id;

            if (id != -1)
            {
                FinanceCategory category = FinanceCategoryManager.Get(id);
                NameInput.Text = category.Name;
                ColorPickerFrame.BackgroundColor = Color.FromHex(category.ColorCode);
            }
        }

        private void ColorPicker_PickedColorChanged(object sender, Color colorPicked)
        {
            if (changeColorCode)
            {
                ColorPickerFrame.BackgroundColor = colorPicked;
            }
            changeColorCode = true;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            NameFrame.BackgroundColor = string.IsNullOrEmpty(NameInput.Text) ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);

            if (!string.IsNullOrEmpty(NameInput.Text))
            {
                FinanceCategory category = new FinanceCategory();
                if (id != -1)
                {
                    category = FinanceCategoryManager.Get(id);
                }
                else
                {
                    category.Id = FinanceCategoryManager.CategoryId++;
                }

                category.Name = NameInput.Text;
                category.ColorCode = ColorPickerFrame.BackgroundColor.ToHex();

                if (id == -1)
                {
                    FinanceCategoryManager.Add(category);
                }

                Database.SaveCategories();

                await Navigation.PopAsync();
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            FinanceCategoryManager.Remove(id);

            await Navigation.PopToRootAsync();
        }
    }
}