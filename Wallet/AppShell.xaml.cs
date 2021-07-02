using System;
using Wallet.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wallet
{
    public partial class AppShell : Shell
    {
        Uri uri;

        public AppShell()
        {
            InitializeComponent();

            ColorManager.InitializeColors();

            SettingsManager.AddCurrency(new Currency { Id = 0, Name = "$", Value = "en-US" });
            SettingsManager.AddCurrency(new Currency { Id = 1, Name = "€", Value = "de-DE" });
            SettingsManager.AddCurrency(new Currency { Id = 2, Name = "£", Value = "en-GB" });
            SettingsManager.AddCurrency(new Currency { Id = 3, Name = "Ft", Value = "hu-HU" });
            SettingsManager.AddCurrency(new Currency { Id = 4, Name = "₽", Value = "ru-RU" });

            SettingsManager.AcitveCurrency = SettingsManager.Currencies[0];


            //Database.ResetDatabase();
            //Database.LoadDefaults();
            Database.LoadFromDatabase();
            //Database.SaveBudgetGoals();

            if (SettingsManager.FirstTime)
            {
                Database.ResetDatabase();
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            uri = new Uri("http://richard.bolya.eu/");
            OpenBrowser(uri);
        }

        public async void OpenBrowser(Uri uri)
        {
            await Browser.OpenAsync(uri);
        }
    }
}
