using System;
using Wallet.Models;
using Xamarin.Forms;

namespace Wallet.Views
{
    public partial class AddFinance : ContentPage
    {
        private bool isExpense = true;
        private FinanceCategory selectedCategory;
        private DateTime selectedDate = DateTime.Now;
        private DateTime? selectedAutomatizedDate = null;

        public AddFinance()
        {
            InitializeComponent();

            SelectDate.Date = selectedDate;

            CategoryPicker.Items.Clear();

            foreach (FinanceCategory category in FinanceCategoryManager.Categories)
            {
                CategoryPicker.Items.Add(category.Name);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (selectedCategory != null && selectedDate != null)
            {
                Finance finance = new Finance();

                finance.Id = FinanceManager.FinanceId++;
                finance.Money = int.Parse(MoneyInput.Text);
                finance.CategoryId = selectedCategory.Id;
                finance.IsExpense = isExpense;
                finance.Date = selectedAutomatizedDate == null ? selectedDate : (DateTime)selectedAutomatizedDate;
                finance.AutomatizedDate = selectedAutomatizedDate;

                FinanceManager.Add(finance);
                Database.SaveFinances();

                await Navigation.PopToRootAsync();
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            isExpense = !e.Value;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCategory = FinanceCategoryManager.Categories[((Picker)sender).SelectedIndex];
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            selectedDate = e.NewDate;
        }

        private void AutomatizedDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            selectedAutomatizedDate = e.NewDate;
        }
    }
}