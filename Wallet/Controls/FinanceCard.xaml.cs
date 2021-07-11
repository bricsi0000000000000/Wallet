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
        private readonly bool isFinance;

        public FinanceCard(Template template, bool isReadonly = false)
        {
            InitializeComponent();

            id = template.Id;
            isFinance = template is Finance;

            DescriptionLabel.Text = template.Description;
            if (isFinance)
            {
                RegularityLabel.Text = (template as Finance).IsAutomatized ? "Regular" : "One time";
            }
            MoneyLabel.Text = $"{(template.Type == FinanceType.Expense ? "-" : "")}{template.Money.FormatToMoney()}";
            MoneyLabel.TextColor = template.Type == FinanceType.Expense ? ColorManager.Expense: ColorManager.Income;
            EditButton.BackgroundColor = FinanceCategoryManager.Get(template.CategoryId).ColorCode.ToColor();
            EditButton.IsEnabled = !isReadonly;
        }

        private void Edit(object sender, System.EventArgs e)
        {
            if (isFinance)
            {
                Navigation.PushAsync(new AddFinance(id));
            }
            else
            {
                Navigation.PushAsync(new AddTemplate(id));
            }
        }
    }
}