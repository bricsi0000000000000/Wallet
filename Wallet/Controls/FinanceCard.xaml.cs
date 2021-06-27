using Wallet.Models;
using Wallet.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FinanceCard : ContentView
    {
        int id;

        public FinanceCard(Finance finance)
        {
            InitializeComponent();

            this.id = finance.Id;

            DescriptionLabel.Text = finance.Description;
            RegularityLabel.Text = finance.IsAutomatized ? "Regular" : "One time";
            DateLabel.Text = finance.Date.ToString("MMM dd, yyyy");
            MoneyLabel.Text = finance.Money.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("hu-HU"));
            EditButton.BackgroundColor = Color.FromHex(FinanceCategoryManager.Get(finance.CategoryId).ColorCode);
        }

        private void EditButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddFinance(id));
        }
    }
}