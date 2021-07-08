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
        private readonly int id;

        public AddCategory(int id = -1)
        {
            InitializeComponent();

            this.id = id;

            //NameFrame.BackgroundColor =
            //NameInput.BackgroundColor = ColorManager.Background;

            //NameInput.TextColor = ColorManager.Text;
            //NameInput.PlaceholderColor = ColorManager.PlaceholderText;

            //ColorPickerFrame.BackgroundColor =
            //ColorPicker.BackgroundColor = ColorManager.Background;

            DeleteImageButton.BackgroundColor = ColorManager.DeleteButton;

            SaveImageButton.BackgroundColor =
            SaveImageButton1.BackgroundColor = ColorManager.Button;

            TitleLabel.Text = id == -1 ? "Add Category" : "Edit Category";

            BothButtonsGrid.IsVisible = id != -1;
            OneButtonGrid.IsVisible = id == -1;

            if (id != -1)
            {
                FinanceCategory category = FinanceCategoryManager.Get(id);
                NameInput.Text = category.Name;
                ColorPicker.SelectedColor = Color.FromHex(category.ColorCode);
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            NameFrame.BackgroundColor = ColorManager.IsInputEmpty(string.IsNullOrEmpty(NameInput.Text));

            if (!string.IsNullOrEmpty(NameInput.Text))
            {
                string name = NameInput.Text.Trim();
                FinanceCategory category = new FinanceCategory();

                if (id == -1)
                {
                    if (FinanceCategoryManager.Categories.Find(x => x.Name.Equals(name)) != null)
                    {
                        await DisplayAlert("Can't add category", $"You have already a category added with name {NameInput.Text}", "Ok");
                        return;
                    }
                    else
                    {
                        category.Id = FinanceCategoryManager.CategoryId++;
                        FinanceCategoryManager.Add(category);
                    }
                }
                else
                {
                    category = FinanceCategoryManager.Get(id);
                }

                category.Name = name;
                category.ColorCode = ColorPicker.SelectedColor.ToHex();

                Database.SaveCategories();

                await Navigation.PopAsync();
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool canDeleteCategory = false;
            List<Finance> finances = FinanceManager.Finances.FindAll(x => x.CategoryId == id);
            List<Budget> budgets = BudgetGoalManager.BudgetGoals.FindAll(x => x.CategoryId == id);
            if (finances.Any() && budgets.Any())
            {
                canDeleteCategory = await DisplayAlert("Can't delete category", $"You have {finances.Count} saved finances and {budgets.Count} saved budget goals with this category.\nWould you like to delete all of them?", "Delete all", "Cancel");
                if (canDeleteCategory)
                {
                    foreach (Finance finance in finances)
                    {
                        FinanceManager.Remove(finance.Id);
                    }
                    Database.SaveFinances();

                    foreach (Budget budget in budgets)
                    {
                        BudgetGoalManager.Remove(budget.Id);
                    }
                    Database.SaveBudgetGoals();
                }
            }
            else if (finances.Any())
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
            else if (budgets.Any())
            {
                canDeleteCategory = await DisplayAlert("Can't delete category", $"You have {budgets.Count} saved budget goals with this category.\nWould you like to delete all of them?", "Delete all", "Cancel");
                if (canDeleteCategory)
                {
                    foreach (Budget budget in budgets)
                    {
                        BudgetGoalManager.Remove(budget.Id);
                    }
                    Database.SaveBudgetGoals();
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