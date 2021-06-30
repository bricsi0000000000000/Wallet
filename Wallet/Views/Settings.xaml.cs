using System;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();

            if (FinanceManager.InitialMoney > 0)
            {
                InitialMoneyInput.Text = FinanceManager.InitialMoney.ToString();
            }

            CurrencyPicker.Items.Clear();

            foreach (Currency currency in SettingsManager.Currencies)
            {
                this.CurrencyPicker.Items.Add(currency.Name);
            }

            this.CurrencyPicker.SelectedIndex = SettingsManager.Currencies.FindIndex(x => x.Id == SettingsManager.AcitveCurrency.Id);
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(InitialMoneyInput.Text))
            {
                FinanceManager.InitialMoney = int.Parse(InitialMoneyInput.Text);
                Database.SaveInitialMoney();
            }
        }

        private async void ResetDatabaseButton_Clicked(object sender, EventArgs e)
        {
            bool canReset = await DisplayAlert("Attention", $"You are about to delete all saved data. Are you sure about that?", "Yes", "Cancel");

            if (canReset)
            {
                Database.ResetDatabase();
            }
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.AcitveCurrency = SettingsManager.Currencies.Find(x => x.Name.Equals(((Picker)sender).SelectedItem.ToString()));
            Database.SaveActiveCurrency();
        }
    }
}