using System;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTemplate : ContentPage
    {
        private readonly int id;
        private FinanceCategory selectedCategory;

        public AddTemplate(int id = -1)
        {
            InitializeComponent();

            this.id = id;

            DeleteImageButton.BackgroundColor = ColorManager.DeleteButton;

            SaveImageButton.BackgroundColor =
            SaveImageButton1.BackgroundColor = ColorManager.Button;

            TitleLabel.Text = id == -1 ? "Add Template" : "Edit Template";
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
                IsExpenseSwitch.IsToggled = true;
            }
            else
            {
                Template template = FinanceManager.GetTemplate(id);
                MoneyInput.Text = template.Money.ToString();
                DescriptionInput.Text = template.Description;
                CategoryPicker.SelectedIndex = template.CategoryId - 1;
                IsExpenseSwitch.IsToggled = template.Type == FinanceType.Expense;
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            CategoryPickerFrame.BorderColor = ColorManager.IsInputEmpty(selectedCategory == null);

            if (!string.IsNullOrEmpty(MoneyInput.Text) && selectedCategory != null)
            {
                Template template = new Template();
                if (id != -1)
                {
                    template = FinanceManager.GetTemplate(id);
                }
                else
                {
                    template.Id = FinanceManager.FinanceId++;
                }

                template.Money = int.Parse(MoneyInput.Text);

                template.Description = string.IsNullOrEmpty(DescriptionInput.Text) ? CategoryPicker.SelectedItem.ToString() : DescriptionInput.Text.Trim();
                template.CategoryId = selectedCategory.Id;
                template.Type = IsExpenseSwitch.IsToggled ? FinanceType.Expense : FinanceType.Income;

                if (id == -1)
                {
                    FinanceManager.AddTemplate(template);
                }

                Database.SaveTemplates();

                await Navigation.PopToRootAsync();
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            FinanceManager.RemoveTemplate(id);

            Database.SaveTemplates();

            await Navigation.PopToRootAsync();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCategory = FinanceCategoryManager.Categories[((Picker)sender).SelectedIndex];
        }

        private async void AddNewCategory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCategory());
        }
    }
}