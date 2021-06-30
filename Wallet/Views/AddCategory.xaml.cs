using System;
using System.Collections.Generic;
using System.Linq;
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

        public AddCategory(int id = -1)
        {
            InitializeComponent();

            this.id = id;

            BothButtonsGrid.IsVisible = id != -1;
            OneButtonGrid.IsVisible = id == -1;

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
                string name = NameInput.Text.Trim();
                if (FinanceCategoryManager.Categories.Find(x => x.Name.Equals(name)) == null)
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

                    category.Name = name;
                    category.ColorCode = ColorPickerFrame.BackgroundColor.ToHex();

                    if (id == -1)
                    {
                        FinanceCategoryManager.Add(category);
                    }

                    Database.SaveCategories();

                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Can't add category", $"You have already a category added with name {NameInput.Text}", "Ok");
                }
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool canDeleteCategory = false;
            List<Finance> finances = FinanceManager.Finances.FindAll(x => x.CategoryId == id);
            if (finances.Any())
            {
                canDeleteCategory = await DisplayAlert("Can't delete category", $"You have {finances.Count} saved finances with this category.\nWould you like to delete all of them?", "Delete all", "Cancel");
                if (canDeleteCategory)
                {
                    foreach (Finance finance in finances)
                    {
                        FinanceManager.Remove(finance.Id);
                    }
                    Database.SaveFinances();
                }
            }
            else
            {
                canDeleteCategory = true;
            }

            if (canDeleteCategory)
            {
                FinanceCategoryManager.Remove(id);
                Database.SaveCategories();
                await Navigation.PopToRootAsync();
            }
        }
    }
}