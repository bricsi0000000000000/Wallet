using System;
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
        private const string GREEN = "#27a555";
        private const string BACKGROUND = "#EBEEF0";

        public AddBudget(int id)
        {
            InitializeComponent();

            this.id = id;

            CategoryPicker.Items.Clear();

            foreach (FinanceCategory category in FinanceCategoryManager.Categories)
            {
                CategoryPicker.Items.Add(category.Name);
            }

            if (id != -1)
            {
                Finance finance = FinanceManager.Get(id);
                CategoryPicker.SelectedIndex = finance.CategoryId - 1;
            }
        }

        private void SelectCategory(object sender, EventArgs e)
        {
            selectedCategory = FinanceCategoryManager.Categories[((Picker)sender).SelectedIndex];
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            //FinanceManager.Remove(id);

            //Database.SaveFinances();

            await Navigation.PopToRootAsync();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            MaxMoneyFrame.BorderColor = string.IsNullOrEmpty(MaxMoneyInput.Text) ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);
            CategoryPickerFrame.BorderColor = selectedCategory == null ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);

            if (!string.IsNullOrEmpty(MaxMoneyInput.Text) && selectedCategory != null)
            {
                await Navigation.PopToRootAsync();
            }
        }
    }
}