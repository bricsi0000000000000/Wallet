using System;
using System.Collections.Generic;
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

            Settings.Currencies = new List<string>();
            Settings.AddCurrency("en-GB");
            Settings.AddCurrency("en-US");
            Settings.AddCurrency("hu-HU");

            Settings.AcitveCurrency = "en-US";


            //Database.ResetDatabase();
            //Database.LoadDefaults();
            Database.LoadFromDatabase();
            //Database.SaveBudgetGoals();

            if (Settings.FirstTime)
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
