using System.Collections.Generic;
using System.Linq;
using Wallet.Controls;
using Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class History : ContentPage
    {
        public History()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ListItems.Children.Clear();

            foreach (IGrouping<(int Year, int Month), Finance> group in FinanceManager.Finances.GroupBy(x => (x.Date.Year, x.Date.Month)))
            {
                int incomeMoney = 0;
                int expenseMoney = 0;
                int mostSpendingCategoryId = -1;
                int mostSpendingCategoryMoney = -1;
                List<Finance> finances = new List<Finance>();

                foreach (Finance finance in group)
                {
                    finances.Add(finance);
                    if (finance.Type == FinanceType.Income)
                    {
                        incomeMoney += finance.Money;
                    }
                }

                foreach (Finance finance in group)
                {
                    if (finance.Type == FinanceType.Expense)
                    {
                        expenseMoney += finance.Money;
                        if (finance.Money > mostSpendingCategoryMoney)
                        {
                            mostSpendingCategoryMoney = finance.Money;
                            mostSpendingCategoryId = finance.CategoryId;
                        }
                    }
                }

                ListItems.Children.Add(new HistoryItemCard(incomeMoney, expenseMoney, new System.DateTime(group.Key.Year, group.Key.Month, 1), FinanceCategoryManager.Get(mostSpendingCategoryId), finances));
            }
        }
    }
}