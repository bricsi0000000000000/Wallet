using System;
using Wallet.Models;
using Xamarin.Forms;

namespace Wallet.Views
{
    public partial class AddFinance : ContentPage
    {
        private bool isExpense = true;
        private FinanceCategory selectedCategory;

        public AddFinance()
        {
            InitializeComponent();

            CategoryPicker.Items.Clear();

            foreach (FinanceCategory category in FinanceCategoryManager.Categories)
            {
                CategoryPicker.Items.Add(category.Name);
            }
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            Finance finance = new Finance
            {
                Id = FinanceManager.FinanceId++,
                Money = int.Parse(MoneyInput.Text),
                CategoryId = selectedCategory.Id,
                IsExpense = isExpense,
                Date = DateTime.Now
            };

            FinanceManager.InsertFront(finance);
            Database.SaveFinances();

            await Navigation.PopToRootAsync();
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            isExpense = !e.Value;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCategory = FinanceCategoryManager.Categories[((Picker)sender).SelectedIndex];
        }
    }
}