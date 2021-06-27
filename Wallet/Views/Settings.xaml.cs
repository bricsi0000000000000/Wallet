using System;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        private const string RED = "#B00020";
        private const string BACKGROUND = "#EBEEF0";

        public Settings()
        {
            InitializeComponent();

            if (FinanceManager.InitialMoney > 0)
            {
                InitialMoneyInput.Text = FinanceManager.InitialMoney.ToString();
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            InitialMoneyFrame.BorderColor = string.IsNullOrEmpty(InitialMoneyInput.Text) ? Color.FromHex(RED) : Color.FromHex(BACKGROUND);

            if (!string.IsNullOrEmpty(InitialMoneyInput.Text))
            {
                FinanceManager.InitialMoney = int.Parse(InitialMoneyInput.Text);
                Database.SaveInitialMoney();
            }
        }

        private void ResetDatabaseButton_Clicked(object sender, EventArgs e)
        {
            Database.ResetDatabase();
        }
    }
}