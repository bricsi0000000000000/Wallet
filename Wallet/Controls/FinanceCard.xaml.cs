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

        private const string RED = "#B00020";
        private const string GREEN = "#27a555";

        public FinanceCard(Finance finance)
        {
            InitializeComponent();

            this.id = finance.Id;

            DescriptionLabel.Text = finance.Description;
            RegularityLabel.Text = finance.IsAutomatized ? "Regular" : "One time";
            MoneyLabel.Text = $"{(finance.Type == FinanceType.Expense || finance.Type == FinanceType.Deposit ? "-" : "")}{finance.Money.FormatToMoney()}";
            EditButton.BackgroundColor = Color.FromHex(FinanceCategoryManager.Get(finance.CategoryId).ColorCode);
        }

        private void EditButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddFinance(id));
        }
    }
}