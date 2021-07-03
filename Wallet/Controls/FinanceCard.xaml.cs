using Wallet.Models;
using Wallet.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FinanceCard : ContentView
    {
        private readonly int id;

        public FinanceCard(Finance finance)
        {
            InitializeComponent();

            this.id = finance.Id;

          //  MainFrame.BackgroundColor = ColorManager.Background;

            //DescriptionLabel.TextColor = RegularityLabel.TextColor = ColorManager.SecondaryText;

            DescriptionLabel.Text = finance.Description;
            RegularityLabel.Text = finance.IsAutomatized ? "Regular" : "One time";
            MoneyLabel.Text = $"{(finance.Type == FinanceType.Expense || finance.Type == FinanceType.Deposit ? "-" : "")}{finance.Money.FormatToMoney()}";
            EditButton.BackgroundColor = FinanceCategoryManager.Get(finance.CategoryId).ColorCode.ToColor();
        }

        private void Edit(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddFinance(id));
        }
    }
}