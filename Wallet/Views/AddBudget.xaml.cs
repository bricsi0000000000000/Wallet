using System;
using System.Linq;
using System.Reflection;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBudget : ContentPage
    {
        FinanceCategory selectedCategory;
        int id;

        private const string RED = "#B00020";
        private const string BACKGROUND = "#EBEEF0";

        public AddBudget(int id = -1)
        {
            InitializeComponent();

            this.id = id;

            BothButtonsGrid.IsVisible = id != -1;
            OneButtonGrid.IsVisible = id == -1;
        }

        protected override void OnAppearing()
        {
            CategoryPicker.Items.Clear();

            foreach (FinanceCategory category in FinanceCategoryManager.Categories)
            {
                if (BudgetGoalManager.BudgetGoals.Find(x => x.CategoryId == category.Id) == null)
                {
                    CategoryPicker.Items.Add(category.Name);
                }
            }

            if (id != -1)
            {
                Budget budget = BudgetGoalManager.Get(id);
                selectedCategory = FinanceCategoryManager.Get(budget.CategoryId);
                string categoryName = FinanceCategoryManager.Get(budget.CategoryId).Name;
                CategoryPicker.Items.Add(categoryName);

                CategoryPicker.SelectedIndex = CategoryPicker.Items.Count;

                MaxMoneyInput.Text = budget.MaxMoney.ToString();
            }
        }

        private void SelectCategory(object sender, EventArgs e)
        {
            selectedCategory = FinanceCategoryManager.Categories.Find(x => x.Name.Equals(((Picker)sender).SelectedItem));
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            BudgetGoalManager.Remove(id);

            Database.SaveBudgetGoals();

            await Navigation.PopToRootAsync();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            MaxMoneyFrame.BorderColor = string.IsNullOrEmpty(MaxMoneyInput.Text) ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);
            CategoryPickerFrame.BorderColor = selectedCategory == null ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);

            if (!string.IsNullOrEmpty(MaxMoneyInput.Text) && selectedCategory != null)
            {
                Budget budget = new Budget();

                if (id != -1)
                {
                    budget = BudgetGoalManager.Get(id);
                }
                else
                {
                    budget.Id = BudgetGoalManager.BudgetGoalId++;
                }

                budget.MaxMoney = int.Parse(MaxMoneyInput.Text);
                budget.CategoryId = selectedCategory.Id;

                budget.SpentMoney = FinanceManager.Finances.FindAll(x => x.CategoryId == selectedCategory.Id &&
                                                                                x.Date.Year == DateTime.Today.Year &&
                                                                                x.Date.Month == DateTime.Today.Month).Sum(x => x.Money);

                if (id == -1)
                {
                    BudgetGoalManager.Add(budget);
                }

                Database.SaveBudgetGoals();

                await Navigation.PopAsync();
            }
        }

        private async void AddNewCategory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCategory());
        }
    }
}