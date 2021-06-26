using System;
using Wallet.Models;
using Xamarin.Forms;

namespace Wallet.Views
{
    public partial class AddFinance : ContentPage
    {
        private int id;
        private FinanceCategory selectedCategory;
        private DateTime selectedDate = DateTime.Now;

        private const string RED = "#B00020";
        private const string GREEN = "#27a555";
        private const string BACKGROUND = "#EBEEF0";

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
                FinanceTypePicker.SelectedIndex = 0;
                IsAutomatizedPicker.SelectedIndex = 1;
            }
            else
            {
                Finance finance = FinanceManager.Get(id);
                MoneyInput.Text = finance.Money.ToString();
                CategoryPicker.SelectedIndex = finance.CategoryId - 1;
                FinanceTypePicker.SelectedIndex = finance.IsExpense ? 0 : 1;
                SelectDate.Date = finance.Date;
                IsAutomatizedPicker.SelectedIndex = finance.IsAutomatized ? 0 : 1;
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            MoneyFrame.BorderColor = string.IsNullOrEmpty(MoneyInput.Text) ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);
            CategoryPickerFrame.BorderColor = selectedCategory == null ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);
            DatePickerFrame.BorderColor = selectedDate == null ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);

            if (!string.IsNullOrEmpty(MoneyInput.Text) && selectedCategory != null && selectedDate != null)
            {
                Finance finance = new Finance();
                if (id != -1)
                {
                    finance = FinanceManager.Get(id);
                }
                else
                {
                    finance.Id = FinanceManager.FinanceId++;
                }

                finance.Money = int.Parse(MoneyInput.Text);
                finance.CategoryId = selectedCategory.Id;
                finance.IsExpense = FinanceTypePicker.SelectedIndex == 0;
                finance.Date = selectedDate;
                finance.IsAutomatized = IsAutomatizedPicker.SelectedIndex == 0;

                if (id == -1)
                {
                    FinanceManager.Add(finance);
                }

                Database.SaveFinances();

                await Navigation.PopToRootAsync();
            }
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCategory = FinanceCategoryManager.Categories[((Picker)sender).SelectedIndex];
            CategoryPickerFrame.BorderColor = Color.FromHex(BACKGROUND);
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            selectedDate = e.NewDate;
        }

        private void FinanceTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            FinanceTypeFrame.BackgroundColor = FinanceTypePicker.SelectedIndex == 0 ? Color.FromHex("#ffffff") : Color.FromHex(GREEN);
        }

        private void IsAutomatizedPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}