using System;
using Wallet.Models;
using Xamarin.Forms;

namespace Wallet.Views
{
    public partial class AddFinance : ContentPage
    {
        private readonly int id;
        private FinanceCategory selectedCategory;
        private DateTime selectedDate = DateTime.Now;
        private bool isAutomatized = false;

        public AddFinance(int id = -1)
        {
            InitializeComponent();

            this.id = id;

            //MoneyFrame.BackgroundColor =
            //DescriptionFrame.BackgroundColor =
            //MoneyInput.BackgroundColor =
            //CategoryPickerFrame.BackgroundColor =
            //CategoryPicker.BackgroundColor =
            //AddNewCategoryImageButton.BackgroundColor =
            //FinanceTypeFrame.BackgroundColor =
            //FinanceTypePicker.BackgroundColor =
            //DatePickerFrame.BackgroundColor =
            //IsAutomatizedFrame.BackgroundColor =
            ////IsAutomatizedPicker.BackgroundColor = 
            //DescriptionInput.BackgroundColor = ColorManager.Background;

            //MoneyInput.TextColor = ColorManager.Text;
            //MoneyInput.PlaceholderColor = ColorManager.PlaceholderText;

            //DescriptionInput.TextColor = ColorManager.Text;
            //DescriptionInput.PlaceholderColor = ColorManager.PlaceholderText;

            //CategoryPicker.TextColor = ColorManager.Text;
            //CategoryPicker.TitleColor = ColorManager.PlaceholderText;

            //FinanceTypePicker.TextColor = ColorManager.Text;
            //FinanceTypePicker.TitleColor = ColorManager.PlaceholderText;

            ////IsAutomatizedPicker.TextColor = ColorManager.Text;
            ////IsAutomatizedPicker.TitleColor = ColorManager.PlaceholderText;

            //IsAutomatizedLabel.TextColor = ColorManager.Text;

            DeleteImageButton.BackgroundColor = ColorManager.DeleteButton;

            SaveImageButton.BackgroundColor =
            SaveImageButton1.BackgroundColor = ColorManager.Button;

            TitleLabel.Text = id == -1 ? "Add Finance" : "Edit Finance";
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
                //IsAutomatizedPicker.SelectedIndex = 1;
                IsAutomatizedSwitch.IsToggled = false;
            }
            else
            {
                Finance finance = FinanceManager.Get(id);
                MoneyInput.Text = finance.Money.ToString();
                DescriptionInput.Text = finance.Description;
                CategoryPicker.SelectedIndex = finance.CategoryId - 1;
                FinanceTypePicker.SelectedIndex = (int)finance.Type;
                SelectDate.Date = finance.Date;
                //IsAutomatizedPicker.SelectedIndex = finance.IsAutomatized ? 0 : 1;
                IsAutomatizedSwitch.IsToggled = finance.IsAutomatized;
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            //MoneyFrame.BorderColor = ColorManager.IsInputEmpty(string.IsNullOrEmpty(MoneyInput.Text));
            CategoryPickerFrame.BorderColor = ColorManager.IsInputEmpty(selectedCategory == null);
            //DatePickerFrame.BorderColor = ColorManager.IsInputEmpty(selectedDate == null);

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

                int money = finance.Money;

                finance.Money = int.Parse(MoneyInput.Text);

                money -= finance.Money;

                finance.Description = string.IsNullOrEmpty(DescriptionInput.Text) ? CategoryPicker.SelectedItem.ToString() : DescriptionInput.Text;
                finance.CategoryId = selectedCategory.Id;
                finance.Type = (FinanceType)FinanceTypePicker.SelectedIndex;
                finance.Date = selectedDate;
                //finance.IsAutomatized = IsAutomatizedPicker.SelectedIndex == 0;
                finance.IsAutomatized = isAutomatized;

                if (id == -1)
                {
                    FinanceManager.Add(finance);
                }
                else
                {
                    BudgetGoalManager.UpdateSpentMoney(finance, money);
                }

                FinanceManager.LoadMonthlyFinances();

                Database.SaveFinances();

                await Navigation.PopToRootAsync();
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            FinanceManager.Remove(id);
            FinanceManager.LoadMonthlyFinances();

            Database.SaveFinances();

            await Navigation.PopToRootAsync();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCategory = FinanceCategoryManager.Categories[((Picker)sender).SelectedIndex];
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            selectedDate = e.NewDate;
        }

        private void FinanceTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FinanceTypeFrame.BackgroundColor = 
            FinanceTypePicker.BackgroundColor = FinanceTypePicker.SelectedIndex == 0 ? ColorManager.Background : ColorManager.Income;
        }

        private async void AddNewCategory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCategory());
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            isAutomatized = e.Value;

            IsAutomatizedLabel.Text = isAutomatized ? "Regular" : "One time";
        }
    }
}