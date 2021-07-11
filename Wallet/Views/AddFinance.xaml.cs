using System;
using System.Collections.Generic;
using Wallet.Controls;
using Wallet.Models;
using Xamarin.Forms;

namespace Wallet.Views
{
    public partial class AddFinance : ContentPage
    {
        private readonly int id;
        private FinanceCategory selectedCategory;
        private DateTime selectedDate = DateTime.Now;
        private bool isTemplate = false;
        private List<int> templatesIds = new List<int>();

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

            TemplatePicker.Items.Clear();
            templatesIds.Clear();
            foreach (Template template in FinanceManager.Templates)
            {
                TemplatePicker.Items.Add(template.ToString());
                templatesIds.Add(template.Id);
            }

            BothButtonsGrid.IsVisible = id != -1;
            OneButtonGrid.IsVisible = id == -1;

            if (id == -1)
            {
                SelectDate.Date = selectedDate;
                IsExpenseSwitch.IsToggled = true;
                IsAutomatizedSwitch.IsToggled = false;
                IsTemplateSwitch.IsToggled = false;
            }
            else
            {
                Update(FinanceManager.Get(id));
            }
        }

        private void Update(Finance finance)
        {
            MoneyInput.Text = finance.Money.ToString();
            DescriptionInput.Text = finance.Description;
            CategoryPicker.SelectedIndex = finance.CategoryId - 1;
            IsExpenseSwitch.IsToggled = finance.Type == FinanceType.Expense;
            SelectDate.Date = finance.Date;
            IsAutomatizedSwitch.IsToggled = finance.IsAutomatized;
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

                finance.Description = string.IsNullOrEmpty(DescriptionInput.Text) ? CategoryPicker.SelectedItem.ToString() : DescriptionInput.Text.Trim();
                finance.CategoryId = selectedCategory.Id;
                finance.Type = IsExpenseSwitch.IsToggled ? FinanceType.Expense : FinanceType.Income;
                finance.Date = selectedDate;
                finance.IsAutomatized = IsAutomatizedSwitch.IsToggled;

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

                if (isTemplate)
                {
                    FinanceManager.AddTemplate(finance);
                }

                Database.SaveTemplates();

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

        private async void AddNewCategory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCategory());
        }

        private void IsTemplateSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            isTemplate = e.Value;
        }

        private void TemplatePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update(new Finance(FinanceManager.GetTemplate(templatesIds[TemplatePicker.SelectedIndex])));
        }
    }
}