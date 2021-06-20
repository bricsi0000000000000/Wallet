using System;
using Wallet.Models;
using Xamarin.Forms;

namespace Wallet.Views
{
    public partial class AddFinance : ContentPage
    {
        private int id;
        private bool isExpense = true;
        private FinanceCategory selectedCategory;
        private DateTime selectedDate = DateTime.Now;
        private DateTime? selectedAutomatizedDate = null;

        public AddFinance(int id)
        {
            InitializeComponent();

            this.id = id;

            CategoryPicker.Items.Clear();

            foreach (FinanceCategory category in FinanceCategoryManager.Categories)
            {
                CategoryPicker.Items.Add(category.Name);
            }

            if (id == -1)
            {
                SelectDate.Date = selectedDate;
            }
            else
            {
                Finance finance = FinanceManager.Get(id);
                MoneyInput.Text = finance.Money.ToString();
                CategoryPicker.SelectedIndex = finance.CategoryId - 1;
                IsIncomeCheckBox.IsChecked = !finance.IsExpense;
                SelectDate.Date = finance.Date;
                AutomatizedDatePicker.Date = finance.AutomatizedDate == null ? DateTime.Now : (DateTime)finance.AutomatizedDate;
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (selectedCategory != null && selectedDate != null)
            {
                if (id == -1)
                {
                    Finance finance = new Finance();

                    finance.Id = FinanceManager.FinanceId++;
                    finance.Money = int.Parse(MoneyInput.Text);
                    finance.CategoryId = selectedCategory.Id;
                    finance.IsExpense = isExpense;
                    finance.Date = selectedAutomatizedDate == null ? selectedDate : (DateTime)selectedAutomatizedDate;
                    finance.AutomatizedDate = selectedAutomatizedDate;

                    FinanceManager.Add(finance);
                }
                else
                {
                    Finance finance = FinanceManager.Get(id);

                    finance.Id = FinanceManager.FinanceId++;
                    finance.Money = int.Parse(MoneyInput.Text);
                    finance.CategoryId = selectedCategory.Id;
                    finance.IsExpense = isExpense;
                    finance.Date = selectedAutomatizedDate == null ? selectedDate : (DateTime)selectedAutomatizedDate;
                    finance.AutomatizedDate = selectedAutomatizedDate;
                }

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