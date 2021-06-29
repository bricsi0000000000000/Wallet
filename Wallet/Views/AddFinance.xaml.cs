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
        }

        protected override void OnAppearing()
        {
            CategoryPicker.Items.Clear();

            foreach (FinanceCategory category in FinanceCategoryManager.Categories)
            {
                CategoryPicker.Items.Add(category.Name);
            }

            BothButtonsGrid.IsVisible = id != -1;
            OneButtonGrid.IsVisible = id == -1;

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
                DescriptionInput.Text = finance.Description;
                CategoryPicker.SelectedIndex = finance.CategoryId - 1;
                FinanceTypePicker.SelectedIndex = (int)finance.Type;
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
                finance.Description = string.IsNullOrEmpty(DescriptionInput.Text) ? CategoryPicker.SelectedItem.ToString() : DescriptionInput.Text;
                finance.CategoryId = selectedCategory.Id;
                finance.Type = (FinanceType)FinanceTypePicker.SelectedIndex;
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

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            FinanceManager.Remove(id);

            Database.SaveFinances();

            await Navigation.PopToRootAsync();
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

        private async void AddNewCategory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCategory(-1));
        }
    }
}