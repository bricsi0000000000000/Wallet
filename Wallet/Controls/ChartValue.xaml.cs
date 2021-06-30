using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartValue : ContentView
    {
        public ChartValue(Finance finance)
        {
            InitializeComponent();

            FinanceCategory category = FinanceCategoryManager.Get(finance.CategoryId);

            CategoryNameLabel.Text = category.Name;
            MoneyLabel.TextColor = Color.FromHex(category.ColorCode);
            MoneyLabel.Text = finance.Money.FormatToMoney();
        }
    }
}